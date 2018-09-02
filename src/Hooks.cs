// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Web;
using System.Web.Hosting;
using static Chessar.HookManager;
using NormalizePathFunc = System.Func<string, bool, int, bool, string>;
using StringToStringFunc = System.Func<string, string>; // AddLongPathPrefix, RemoveLongPathPrefix

namespace Chessar
{
    /// <summary>
    /// Hooks for support long paths.
    /// </summary>
    public static partial class Hooks
    {
        #region Fields

        private static readonly BindingFlags
            privateStatic = BindingFlags.NonPublic | BindingFlags.Static,
            privateInstance = BindingFlags.NonPublic | BindingFlags.Instance;
        private static readonly Type
            tPath = typeof(Path),
            tString = typeof(string),
            tBool = typeof(bool),
            tInt = typeof(int),
            tHttpResponse = typeof(HttpResponse),
            tUri = typeof(Uri);

        #region Lazy Delegates

        private static readonly Lazy<NormalizePathFunc>
            normalizePath4 = new Lazy<NormalizePathFunc>(() => ToDelegate<NormalizePathFunc>(
                tPath, "NormalizePath", privateStatic, tString, tBool, tInt, tBool));
        private static readonly Lazy<StringToStringFunc>
            addLongPathPrefix = new Lazy<StringToStringFunc>(() => ToDelegate<StringToStringFunc>(
                tPath, "AddLongPathPrefix", privateStatic, tString)),
            removeLongPathPrefix = new Lazy<StringToStringFunc>(() => ToDelegate<StringToStringFunc>(
                tPath, "RemoveLongPathPrefix", privateStatic, tString));

        #endregion

        #region Lazy Reflection

        private static Lazy<Type>
            win32NativeType = new Lazy<Type>(() => Type.GetType("Microsoft.Win32.Win32Native"));

        private static readonly Lazy<MethodInfo[]> originals = new Lazy<MethodInfo[]>(() => new MethodInfo[]
        {
            GetMethod(tPath, "NormalizePath", privateStatic, tString, tBool, tInt),
            GetMethod(tPath, "GetFullPathInternal", privateStatic),
            GetMethod(win32NativeType.Value, "MoveFile", privateStatic),
            GetMethod(win32NativeType.Value, "GetSecurityInfoByName", privateStatic),
            GetMethod(tHttpResponse, "GetNormalizedFilename", privateInstance),
            GetMethod(typeof(Image).Assembly.GetType("System.Drawing.SafeNativeMethods+Gdip"), "GdipSaveImageToFile", privateStatic),
            GetMethod(tUri, "CreateThis", privateInstance)
        });

        private static readonly Lazy<MethodInfo>
            uriParseScheme = new Lazy<MethodInfo>(() => GetMethod(tUri, "ParseScheme", privateStatic)),
            uriInitializeUri = new Lazy<MethodInfo>(() => GetMethod(tUri, "InitializeUri", privateInstance));
        private static Lazy<FieldInfo>
            mStringUriFld = new Lazy<FieldInfo>(() => tUri.GetField("m_String", privateInstance)),
            mFlagsUriFld = new Lazy<FieldInfo>(() => tUri.GetField("m_Flags", privateInstance)),
            mSyntaxUriFld = new Lazy<FieldInfo>(() => tUri.GetField("m_Syntax", privateInstance));
        internal static Lazy<PropertyInfo>
            responseContext = new Lazy<PropertyInfo>(() => tHttpResponse.GetProperty("Context", privateInstance));

        #endregion

        #endregion

        #region Public

        /// <summary>
        /// Patch <see langword="internal/private static"/>
        /// methods in <see cref="Path"/> class (and others)
        /// using JMP hook for support long paths.
        /// Patched method adding long path prefix <see langword="\\?\"/>
        /// (or <see langword="\\?\UNC\"/> for network shares) if needed.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If no methods are found for the patches.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If one or more methods are already patched.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// If a native call fails. This is unrecoverable.
        /// </exception>
        public static void PatchLongPaths()
        {
            MethodInfo[] methods = null;
            try
            {
                methods = originals.Value;
                var len = methods.Length;
                var thisType = typeof(Hooks);
                for (int i = 0; i < len; ++i)
                {
                    var method = methods[i];
                    Hook(method, thisType.GetMethod(method.Name + "Patched", privateStatic));
                }
            }
            catch
            {
                BatchUnhook(methods);
                throw;
            }
        }

        /// <summary>
        /// Remove long path support patches.
        /// </summary>
        /// <exception cref="AggregateException">
        /// If a native call fails. This is unrecoverable.
        /// </exception>
        public static void RemoveLongPathsPatch()
        {
            var errors = BatchUnhook(originals.Value);
            if (errors != null)
                throw errors;
        }

        /// <summary>
        /// Add long path prefix (<see langword="\\?\"/> or <see langword="\\?\UNC\"/>).
        /// </summary>
        /// <param name="path">Path.</param>
        /// <returns>Path with long prefix.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AddLongPathPrefix(this string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));
            Contract.EndContractBlock();

            return addLongPathPrefix.Value(path);
        }

        /// <summary>
        /// Remove long path prefix (<see langword="\\?\"/> or <see langword="\\?\UNC\"/>) if present.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <returns>Path without long prefix.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveLongPathPrefix(this string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));
            Contract.EndContractBlock();

            return removeLongPathPrefix.Value(path);
        }

        #endregion

        #region Patches (required NoInlining)

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string NormalizePathPatched(string path, bool fullCheck, int maxPathLength)
        {
            var normalizedPath = normalizePath4.Value(path, fullCheck, maxPathLength, true);

            // fix for extend path with long prefix
            if (fullCheck && short.MaxValue == maxPathLength)
                return AddLongPathPrefix(normalizedPath);

            return normalizedPath;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetFullPathInternalPatched(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));
            Contract.EndContractBlock();

            var sti = GetStackTrace(2);
            var st = sti.Item1;
            var cType = sti.Item2;
            var ccTypeFullName = sti.Item3;
            var isGetTempPath = sti.Item4;
            var needRemoveLongPrefix = false;

            var needPatch = !isGetTempPath && st != null && NeedPatch(cType, ccTypeFullName, out needRemoveLongPrefix);

            TraceGetFullPathInternalPatchedInfo(st, cType, in needPatch);

            var newPath = needPatch
                ? NormalizePathPatched(path, true, short.MaxValue)
                : normalizePath4.Value(path, true, short.MaxValue, true);

            if (needRemoveLongPrefix)
                newPath = newPath.RemoveLongPathPrefix();

            return newPath;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static uint GetSecurityInfoByNamePatched(
            string name,
            uint objectType,
            uint securityInformation,
            out IntPtr sidOwner,
            out IntPtr sidGroup,
            out IntPtr dacl,
            out IntPtr sacl,
            out IntPtr securityDescriptor) => NativeMethods.GetSecurityInfoByName(
                AddLongPathPrefix(name),
                objectType,
                securityInformation,
                out sidOwner,
                out sidGroup,
                out dacl,
                out sacl,
                out securityDescriptor);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MoveFilePatched(string src, string dst)
            => NativeMethods.MoveFile(AddLongPathPrefix(src), AddLongPathPrefix(dst));

        [Pure, MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetNormalizedFilenamePatched(HttpResponse response, string fn)
        {
            // If it's not a physical path, call MapPath on it
            if (!IsAbsolutePhysicalPath(fn))
            {
                var request = (responseContext.Value.GetValue(response) as HttpContext)?.Request;
                fn = request != null ? request.MapPath(fn) : HostingEnvironment.MapPath(fn);
            }

            return AddLongPathPrefix(fn);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GdipSaveImageToFilePatched(HandleRef image, string filename,
            ref Guid classId, HandleRef encoderParams) => NativeMethods.GdipSaveImageToFile(
                image, AddLongPathPrefix(filename), ref classId, encoderParams);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void CreateThisPatched(Uri thisUri, string uri, bool dontEscape, UriKind uriKind)
        {
            uri = RemoveLongPathPrefix(uri);
            mStringUriFld.Value.SetValue(thisUri, uri);
            var flagsLong = (ulong)mFlagsUriFld.Value.GetValue(thisUri);
            if (dontEscape)
                flagsLong |= 0x00080000;
            var flags = Enum.Parse(mFlagsUriFld.Value.FieldType, flagsLong.ToString(CultureInfo.InvariantCulture));
            var mSyntax = mSyntaxUriFld.Value.GetValue(thisUri);

            object[] args = { uri, flags, mSyntax };
            var err = uriParseScheme.Value.Invoke(null, args);

            mFlagsUriFld.Value.SetValue(thisUri, args[1]);
            mSyntaxUriFld.Value.SetValue(thisUri, args[2]);

            args = new[] { err, uriKind, null };
            uriInitializeUri.Value.Invoke(thisUri, args);

            if (args[2] is UriFormatException e)
                throw e;
        }

        #endregion

        #region Utils

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static Tuple<StackTrace, Type, string, bool> GetStackTrace(in int skipFrames)
        {
            StackTrace st = null;
            Type cType = null;
            string ccTypeFullName = null;
            var isGetTempPath = false;
            try
            {
                st = new StackTrace(skipFrames, false);
                var cMethod = st.GetFrame(0)?.GetMethod();
                cType = cMethod?.DeclaringType;
                isGetTempPath = (cType == tPath && string.Equals("GetTempPath", cMethod.Name)); //-V3105
                if (!isGetTempPath)
                    ccTypeFullName = st.GetFrame(1)?.GetMethod().DeclaringType.FullName;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                TraceGetFullPathInternalPatchedError(ex, cType);
            }

            return new Tuple<StackTrace, Type, string, bool>(st, cType, ccTypeFullName, isGetTempPath);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        internal static MethodInfo GetMethod(Type type, string name, BindingFlags bindingAttr)
            => GetMethod(type, name, bindingAttr, null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MethodInfo GetMethod(Type type, string name, BindingFlags bindingAttr, params Type[] parameters)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            Contract.EndContractBlock();

            var method = parameters is null ? type.GetMethod(name, bindingAttr) : type.GetMethod(name, bindingAttr, null, parameters, null);
            if (method is null)
                throw new MissingMethodException(string.Format(CultureInfo.InvariantCulture,
                    "Method '{0}.{1}' not found", type.FullName, name));

            return method;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T ToDelegate<T>(Type type, string name, BindingFlags bindingAttr, params Type[] parameters)
            where T : Delegate => (T)GetMethod(type, name, bindingAttr, parameters).CreateDelegate(typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsAbsolutePhysicalPath(string path)
        {
            if (path is null || path.Length < 3)
                return false;
            // e.g c:\foo
            if (path[1] == ':' && IsDirectorySeparatorChar(path[2]))
                return true;
            // e.g \\server\share\foo or //server/share/foo
            return IsDirectorySeparatorChar(path[0]) && IsDirectorySeparatorChar(path[1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsDirectorySeparatorChar(char ch) => ch == '\\' || ch == '/';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool NeedPatch(Type t, string fn, out bool needRemoveLongPrefix)
        {
            needRemoveLongPrefix = false;
            return (t is null) ||
                // exclude types for patch, see references to Path.GetFullPathInternal
                // https://referencesource.microsoft.com/#mscorlib/system/io/path.cs,72f9fabbc9d544a5,references
                !(
                    // StringExpressionSet (https://referencesource.microsoft.com/#mscorlib/system/security/util/stringexpressionset.cs,755)
                    (needRemoveLongPrefix = string.Equals(t.Name, "StringExpressionSet")) ||

                    // System.Reflection Namespace
                    t.FullName.StartsWith("System.Reflection.", StringComparison.OrdinalIgnoreCase) || (

                    // Path
                    t == tPath && fn != null && fn.StartsWith("System.Web.", StringComparison.Ordinal))
                );
        }

        [Conditional("TRACE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void TraceGetFullPathInternalPatchedInfo(StackTrace st, Type ct, in bool patched)
        {
            if (ct?.IsSubclassOf(typeof(TraceListener)) ?? false)
                return;
            Trace.TraceInformation("[GetFullPathInternal Patched = {0}]:\r\n{1}", patched, st?.ToString() ?? "<no StackTrace>");
            Trace.Flush();
        }

        [Conditional("TRACE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void TraceGetFullPathInternalPatchedError(Exception ex, Type ct)
        {
            if (ex is null || (ct?.IsSubclassOf(typeof(TraceListener)) ?? false))
                return;
            Trace.TraceError("[GetFullPathInternal StackTrace Exception]:\r\n{0}", ex);
            Trace.Flush();
        }

        #endregion

        #region Natives

        private static class NativeMethods
        {
            [DllImport(
                "advapi32.dll",
                EntryPoint = "GetNamedSecurityInfoW",
                CallingConvention = CallingConvention.Winapi,
                SetLastError = true,
                ExactSpelling = true,
                CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern uint GetSecurityInfoByName(
               string name,
               uint objectType,
               uint securityInformation,
               out IntPtr sidOwner,
               out IntPtr sidGroup,
               out IntPtr dacl,
               out IntPtr sacl,
               out IntPtr securityDescriptor);

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false)]
            [return: MarshalAs(UnmanagedType.Bool)]
            [ResourceExposure(ResourceScope.Machine)]
            internal static extern bool MoveFile(string src, string dst);

            [DllImport("gdiplus.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            internal static extern int GdipSaveImageToFile(HandleRef image, string filename,
                ref Guid classId, HandleRef encoderParams);
        }

        #endregion
    }
}