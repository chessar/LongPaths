// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Web;
using System.Web.Hosting;
using static Chessar.HookManager;
using NormalizePathFunc = System.Func<string, bool, int, bool, string>;
using StringToBoolFunc = System.Func<string, bool>; // IsPathRooted
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

        #region Lazy Delegates

        private static readonly Lazy<NormalizePathFunc>
            normalizePath4 = new Lazy<NormalizePathFunc>(() =>
                (NormalizePathFunc)typeof(Path).GetMethod("NormalizePath", privateStatic, null,
                    new[] { typeof(string), typeof(bool), typeof(int), typeof(bool) }, null)
                        .CreateDelegate(typeof(NormalizePathFunc)));
        private static readonly Lazy<StringToStringFunc>
            addLongPathPrefix = new Lazy<StringToStringFunc>(() =>
                (StringToStringFunc)typeof(Path).GetMethod("AddLongPathPrefix", privateStatic, null,
                    new[] { typeof(string) }, null).CreateDelegate(typeof(StringToStringFunc))),
            removeLongPathPrefix = new Lazy<StringToStringFunc>(() =>
                (StringToStringFunc)typeof(Path).GetMethod("RemoveLongPathPrefix", privateStatic, null,
                    new[] { typeof(string) }, null).CreateDelegate(typeof(StringToStringFunc)));
        internal static readonly Lazy<StringToBoolFunc>
            IsPathRooted = new Lazy<StringToBoolFunc>(() =>
                (StringToBoolFunc)Type.GetType("System.IO.LongPath").GetMethod("IsPathRooted", privateStatic)
                    .CreateDelegate(typeof(StringToBoolFunc)));

        #endregion

        #region Lazy Reflection

        private static Lazy<Type>
            win32NativeType = new Lazy<Type>(() => Type.GetType("Microsoft.Win32.Win32Native"));

        private static readonly Lazy<MethodInfo[]> originals = new Lazy<MethodInfo[]>(() => new MethodInfo[]
        {
            typeof(Path).GetMethod("NormalizePath", privateStatic, null, new[] { typeof(string), typeof(bool), typeof(int) }, null),
            typeof(Path).GetMethod("GetFullPathInternal", privateStatic),
            win32NativeType.Value?.GetMethod("GetSecurityInfoByName", privateStatic),
            win32NativeType.Value?.GetMethod("MoveFile", privateStatic),
            typeof(HttpResponse).GetMethod("GetNormalizedFilename", privateInstance),
            typeof(Image).Assembly.GetType("System.Drawing.SafeNativeMethods+Gdip")?.GetMethod("GdipSaveImageToFile", privateStatic),
            typeof(Uri).GetMethod("CreateThis", privateInstance)
        });

        private static readonly Lazy<MethodInfo>
            uriParseScheme = new Lazy<MethodInfo>(() => typeof(Uri).GetMethod("ParseScheme", privateStatic)),
            uriInitializeUri = new Lazy<MethodInfo>(() => typeof(Uri).GetMethod("InitializeUri", privateInstance));
        private static Lazy<FieldInfo>
            mStringUriFld = new Lazy<FieldInfo>(() => typeof(Uri).GetField("m_String", privateInstance)),
            mFlagsUriFld = new Lazy<FieldInfo>(() => typeof(Uri).GetField("m_Flags", privateInstance)),
            mSyntaxUriFld = new Lazy<FieldInfo>(() => typeof(Uri).GetField("m_Syntax", privateInstance)),
            _context = new Lazy<FieldInfo>(() => typeof(HttpResponse).GetField("_context", privateInstance));

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
                var len = methods?.Length ?? 0;
                if (len == 0)
                    throw new ArgumentException("There are no methods for patch.");
                var thisType = typeof(Hooks);
                for (int i = 0; i < len; ++i)
                {
                    var method = methods[i];
                    if (method is null)
                        throw new ArgumentNullException(nameof(method) + i.ToString(CultureInfo.InvariantCulture));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AddLongPathPrefix(this string path)
        {
            try
            {
                return addLongPathPrefix.Value is null
                    ? path : addLongPathPrefix.Value(path);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        /// <summary>
        /// Remove long path prefix (<see langword="\\?\"/> or <see langword="\\?\UNC\"/>) if present.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <returns>Path without long prefix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveLongPathPrefix(this string path)
        {
            try
            {
                return removeLongPathPrefix.Value is null
                    ? path : removeLongPathPrefix.Value(path);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #endregion

        #region Patches (required NoInlining)

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string NormalizePathPatched(string path, bool fullCheck, int maxPathLength)
        {
            var normalizedPath = NormalizePath4(path, fullCheck, maxPathLength);

            // fix for extend path with long prefix
            if (fullCheck && short.MaxValue == maxPathLength)
                return AddLongPathPrefix(normalizedPath);

            return normalizedPath;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetFullPathInternalPatched(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));
            Contract.EndContractBlock();

            StackTrace st = null;
            Type cType = null;
            string ccTypeFullName = null;
            var isGetTempPath = false;
            try
            {
                st = new StackTrace(1, false);
                if (st.FrameCount > 0)
                {
                    var cMethod = st.GetFrame(0)?.GetMethod();
                    cType = cMethod?.DeclaringType;
                    isGetTempPath = (cType == typeof(Path) && string.Equals("GetTempPath", cMethod?.Name));
                    if (!isGetTempPath && st.FrameCount > 1)
                        ccTypeFullName = st.GetFrame(1)?.GetMethod()?.DeclaringType?.FullName;
                }
            }
            catch (Exception ex)
            {
                TraceGetFullPathInternalPatchedError(ex, cType);
            }

            var needPatch = !isGetTempPath && st != null && NeedPatch(cType, ccTypeFullName);

            TraceGetFullPathInternalPatchedInfo(st, cType, in needPatch); // comment 'in' for PVS

            var newPath = needPatch
                ? NormalizePathPatched(path, true, short.MaxValue)
                : NormalizePath4(path, true, short.MaxValue);

            if (string.Equals(cType?.Name, "StringExpressionSet"))
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
                name?.AddLongPathPrefix(),
                objectType,
                securityInformation,
                out sidOwner,
                out sidGroup,
                out dacl,
                out sacl,
                out securityDescriptor);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MoveFilePatched(string src, string dst)
            => NativeMethods.MoveFile(src?.AddLongPathPrefix(), dst?.AddLongPathPrefix());

        [Pure, MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetNormalizedFilenamePatched(HttpResponse response, string fn)
        {
            try
            {
                // If it's not a physical path, call MapPath on it
                if (!IsPathRooted.Value(fn))
                {
                    var request = (_context.Value?.GetValue(response) as HttpContext)?.Request;
                    fn = request != null ? request.MapPath(fn) : HostingEnvironment.MapPath(fn);
                }

                return fn.AddLongPathPrefix();
            }
            catch (NullReferenceException)
            {
                throw new MissingMethodException("Method System.IO.LongPath.IsPathRooted(string) not found.");
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GdipSaveImageToFilePatched(HandleRef image, string filename,
            ref Guid classId, HandleRef encoderParams) => NativeMethods.GdipSaveImageToFile(
                image, filename?.AddLongPathPrefix(), ref classId, encoderParams);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void CreateThisPatched(Uri thisUri, string uri, bool dontEscape, UriKind uriKind)
        {
            try
            {
                uri = uri?.RemoveLongPathPrefix();
                mStringUriFld.Value.SetValue(thisUri, uri ?? string.Empty);
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
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #endregion

        #region Utils

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string NormalizePath4(string path, bool fullCheck, int maxPathLength)
        {
            try
            {
                return normalizePath4.Value(path, fullCheck, maxPathLength, true);
            }
            catch (NullReferenceException)
            {
                throw new MissingMethodException("Method System.IO.Path.NormalizePath(string, bool, int, bool) not found.");
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool NeedPatch(Type t, string fn) => t is null ||
        // exclude types for patch, see references to Path.GetFullPathInternal
        // https://referencesource.microsoft.com/#mscorlib/system/io/path.cs,72f9fabbc9d544a5,references
        !(
            // StringExpressionSet (https://referencesource.microsoft.com/#mscorlib/system/security/util/stringexpressionset.cs,755)
            string.Equals(t.Name, "StringExpressionSet") ||

            // System.Reflection Namespace
            t.FullName.StartsWith("System.Reflection.", StringComparison.OrdinalIgnoreCase) || (

            // Path
            t == typeof(Path) && fn != null && fn.StartsWith("System.Web.", StringComparison.Ordinal))
        );

        [Conditional("TRACE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void TraceGetFullPathInternalPatchedInfo(StackTrace st, Type ct, in bool patched) // comment 'in' for PVS
        {
            if (ct?.IsSubclassOf(typeof(TraceListener)) ?? false)
                return;
            Trace.TraceInformation("[GetFullPathInternal Patched = {0}]:\r\n{1}", patched, st?.ToString() ?? "<no StackTrace>");
            Trace.Flush();
        }

        [Conditional("TRACE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void TraceGetFullPathInternalPatchedError(Exception ex, Type ct)
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