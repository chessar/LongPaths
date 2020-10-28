// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.Hosting;
using static Chessar.HookManager;
using LongPathDirectoryMoveAction = System.Action<string, string>;
using NormalizePathFunc = System.Func<string, bool, int, bool, string>;
using StringParamsToStringFunc = System.Func<string, object[], string>; // GetResourceString
using StringToStringFunc = System.Func<string, string>; // AddLongPathPrefix, RemoveLongPathPrefix

namespace Chessar
{
    /// <summary>
    /// Hooks for support long paths.
    /// </summary>
    public static partial class Hooks
    {
        #region Consts

        // Error codes from WinError.h
        private const int
            ERROR_SUCCESS = 0x0,
            ERROR_FILE_NOT_FOUND = 0x2,
            ERROR_ACCESS_DENIED = 0x5,
            ERROR_INVALID_PARAMETER = 0x57,
            ERROR_INVALID_NAME = 0x7B,
            ERROR_INVALID_OWNER = 0x51B,
            ERROR_INVALID_PRIMARY_GROUP = 0x51C,
            ERROR_NO_SECURITY_ON_OBJECT = 0x546;

        private const int devicePrefixLength = 4;
        private const string
            testString = @"\\?\c:/\d",
            testResultString = @"\\?\c:\d";

        #endregion

        #region Fields

        private static readonly BindingFlags
            privateStatic = BindingFlags.NonPublic | BindingFlags.Static,
            privateInstance = BindingFlags.NonPublic | BindingFlags.Instance;
        private static readonly Type
            tPath = typeof(Path),
            tFile = typeof(File),
            tString = typeof(string),
            tBool = typeof(bool),
            tInt = typeof(int),
            tHttpResponse = typeof(HttpResponse),
            tUri = typeof(Uri),
            tEnv = typeof(Environment),
            tDir = typeof(Directory);

        #region Lazy Delegates

        private static readonly Lazy<NormalizePathFunc>
            normalizePath4 = new Lazy<NormalizePathFunc>(() => ToDelegate<NormalizePathFunc>(
                tPath, "NormalizePath", privateStatic, tString, tBool, tInt, tBool));
        private static readonly Lazy<StringToStringFunc>
            addLongPathPrefix = new Lazy<StringToStringFunc>(() => ToDelegate<StringToStringFunc>(
                tPath, "AddLongPathPrefix", privateStatic, tString)),
            removeLongPathPrefix = new Lazy<StringToStringFunc>(() => ToDelegate<StringToStringFunc>(
                tPath, "RemoveLongPathPrefix", privateStatic, tString)),
            getFullPathInternal = new Lazy<StringToStringFunc>(() => ToDelegate<StringToStringFunc>(
                tPath, "GetFullPathInternal", privateStatic, tString)),
            envGetResourceString1 = new Lazy<StringToStringFunc>(() => ToDelegate<StringToStringFunc>(
                tEnv, "GetResourceString", privateStatic, tString));
        private static readonly Lazy<LongPathDirectoryDeleteHelper>
            longPathDirectoryDeleteHelper = new Lazy<LongPathDirectoryDeleteHelper>(() => ToDelegate<LongPathDirectoryDeleteHelper>(
                Type.GetType("System.IO.LongPathDirectory"), "DeleteHelper", privateStatic, tString, tString, tBool, tBool));
        private static readonly Lazy<StringParamsToStringFunc>
            envGetResourceString2 = new Lazy<StringParamsToStringFunc>(() => ToDelegate<StringParamsToStringFunc>(
                tEnv, "GetResourceString", privateStatic, tString, typeof(object[])));
        private static readonly Lazy<LongPathDirectoryMoveAction>
            longPathDirectoryMove = new Lazy<LongPathDirectoryMoveAction>(() => ToDelegate<LongPathDirectoryMoveAction>(
                Type.GetType("System.IO.LongPathDirectory"), "Move", privateStatic, tString, tString));

        #endregion

        #region Lazy Reflection

        private static readonly Lazy<MethodInfo[]> originals = new Lazy<MethodInfo[]>(() => new MethodInfo[]
        {
            GetMethod(tPath, "NormalizePath", privateStatic, tString, tBool, tInt),
            getFullPathInternal.Value.Method,
            GetMethod(tDir, "Delete", privateStatic, tString, tString, tBool, tBool),
            GetMethod(tDir, "InternalMove", privateStatic, tString, tString, tBool),
            GetMethod(typeof(NativeObjectSecurity), "CreateInternal", privateStatic),
            GetMethod(tHttpResponse, "GetNormalizedFilename", privateInstance),
            GetMethod(tUri, "CreateThis", privateInstance)
        });

        private static readonly Lazy<MethodInfo>
            uriParseScheme = new Lazy<MethodInfo>(() => GetMethod(tUri, "ParseScheme", privateStatic)),
            uriInitializeUri = new Lazy<MethodInfo>(() => GetMethod(tUri, "InitializeUri", privateInstance)),
            win32GetSecurityInfo = new Lazy<MethodInfo>(() => GetMethod(
                Type.GetType("System.Security.AccessControl.Win32"), "GetSecurityInfo", privateStatic));
        private static readonly Lazy<FieldInfo>
            mStringUriFld = new Lazy<FieldInfo>(() => tUri.GetField("m_String", privateInstance)),
            mFlagsUriFld = new Lazy<FieldInfo>(() => tUri.GetField("m_Flags", privateInstance)),
            mSyntaxUriFld = new Lazy<FieldInfo>(() => tUri.GetField("m_Syntax", privateInstance));
        internal static Lazy<PropertyInfo>
            responseContext = new Lazy<PropertyInfo>(() => tHttpResponse.GetProperty("Context", privateInstance));
        private static readonly Lazy<ConstructorInfo> csdCtor = new Lazy<ConstructorInfo>(() =>
            typeof(CommonSecurityDescriptor).GetConstructors(privateInstance)
                .First(ci => ci.GetParameters().Length == 4));

        #endregion

        #endregion

        #region Delegates

        private delegate void LongPathDirectoryDeleteHelper(
            string fullPath, string userPath, bool recursive, bool throwOnTopLevelDirectoryNotFound);

        private delegate Exception ExceptionFromErrorCode(
            int errorCode, string name, SafeHandle handle, object context);

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
        [HandleProcessCorruptedStateExceptions]
        public static void PatchLongPaths()
        {
            if (NoPatchRequired())
                return;

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
        /// <exception cref="ArgumentException">
        /// If only one method from the list was not patched earlier.
        /// </exception>
        /// <exception cref="AggregateException">
        /// If more one methods from the list was not patched earlier,
        /// or if a native call fails (this is unrecoverable).
        /// </exception>
        [HandleProcessCorruptedStateExceptions]
        public static void RemoveLongPathsPatch()
        {
            if (!NoPatchRequired())
                return;
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
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AddLongPathPrefix(this string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));
            Contract.EndContractBlock();

            return addLongPathPrefix.Value(path);
        }

        /// <summary>
        /// Add long path prefix (<see langword="\\?\"/> or <see langword="\\?\UNC\"/>)
        /// and delete consecutive directory path chars.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <returns>Path with long prefix and without duplicate directory path chars.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AddLongPathPrefixAndFixSeparators(this string path)
            => FixPathSeparators(AddLongPathPrefix(path));

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

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        [MethodImpl(MethodImplOptions.NoInlining)] // required
        private static void InternalMovePatched(string sourceDirName, string destDirName, bool checkHost)
            => longPathDirectoryMove.Value(FixPathSeparators(sourceDirName), FixPathSeparators(destDirName));

        [MethodImpl(MethodImplOptions.NoInlining)] // required
        [HandleProcessCorruptedStateExceptions]
        private static CommonSecurityDescriptor CreateInternalPatched(ResourceType resourceType,
            bool isContainer, string name, SafeHandle handle, AccessControlSections includeSections,
            bool createByName, ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext)
        {
            if (createByName && name is null)
                throw new ArgumentNullException(nameof(name));
            else if (!createByName && handle is null)
                throw new ArgumentNullException(nameof(handle));
            Contract.EndContractBlock();

            try
            {
                object[] parameters = { resourceType, AddLongPathPrefixAndFixSeparators(name), handle, includeSections, null };
                var error = (int)win32GetSecurityInfo.Value.Invoke(null, parameters);

                if (error != ERROR_SUCCESS)
                {
                    Exception exception = null;

                    if (exceptionFromErrorCode != null)
                        exception = exceptionFromErrorCode(error, name, handle, exceptionContext);

                    if (exception is null)
                    {
                        if (error == ERROR_ACCESS_DENIED)
                            exception = new UnauthorizedAccessException();
                        else if (error == ERROR_INVALID_OWNER)
                            exception = new InvalidOperationException(EnvGetResString1("AccessControl_InvalidOwner"));
                        else if (error == ERROR_INVALID_PRIMARY_GROUP)
                            exception = new InvalidOperationException(EnvGetResString1("AccessControl_InvalidGroup"));
                        else if (error == ERROR_INVALID_PARAMETER)
                            exception = new InvalidOperationException(EnvGetResString2("AccessControl_UnexpectedError", error));
                        else if (error == ERROR_INVALID_NAME)
                            exception = new ArgumentException(EnvGetResString2("Argument_InvalidName"), nameof(name));
                        else if (error == ERROR_FILE_NOT_FOUND)
                            exception = (name is null ? new FileNotFoundException() : new FileNotFoundException(name));
                        else if (error == ERROR_NO_SECURITY_ON_OBJECT)
                            exception = new NotSupportedException(EnvGetResString1("AccessControl_NoAssociatedSecurity"));
                        else
                        {
                            Contract.Assert(false, string.Format(CultureInfo.InvariantCulture, "Win32GetSecurityInfo() failed with unexpected error code {0}", error));
                            exception = new InvalidOperationException(EnvGetResString2("AccessControl_UnexpectedError", error));
                        }
                    }

                    throw exception;
                }

                var rawSD = (RawSecurityDescriptor)parameters[parameters.Length - 1];
                return (CommonSecurityDescriptor)csdCtor.Value
                    .Invoke(new object[] { isContainer, false, rawSD, true });
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
            catch (NullReferenceException nre)
            {
                if (win32GetSecurityInfo.Value is null)
                    throw new MissingMethodException("Method 'System.Security.AccessControl.Win32.GetSecurityInfo' not found.", nre.InnerException ?? nre);
                else if (csdCtor.Value is null)
                    throw new MissingMethodException("Constructor 'CommonSecurityDescriptor' with 4 args not found.", nre.InnerException ?? nre);
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string NormalizePathPatched(string path, bool fullCheck, int maxPathLength)
        {
            var normalizedPath = normalizePath4.Value(path, fullCheck, maxPathLength, true);

            // fix for extend path with long prefix
            if (fullCheck && short.MaxValue == maxPathLength)
                normalizedPath = AddLongPathPrefix(normalizedPath);

            if (IsExtended(normalizedPath))
                normalizedPath = FixPathSeparators(normalizedPath);

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
        [SuppressMessage("Microsoft.Usage", "CA1801: Review unused parameters")]
        private static void DeletePatched(string fullPath, string userPath, bool recursive, bool checkHost)
            => longPathDirectoryDeleteHelper.Value(fullPath, userPath, recursive, true);

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

        [MethodImpl(MethodImplOptions.NoInlining)] // required
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

        /// <summary>
        /// Checking whether a patch is required to support long paths.
        /// </summary>
        /// <returns>
        /// <see langword="True" /> if the patch is not required (already applied),
        /// otherwise - <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.Synchronized)]
        public static bool NoPatchRequired() => testResultString == getFullPathInternal.Value(testString);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string FixPathSeparators(string path)
        {
            var sb = new StringBuilder();
            sb.Append(path[0]).Append(path[1]);
            var len = path.Length;
            char ch;
            bool prevIsSep = false, curIsSep = false;
            var buf = new StringBuilder();
            for (int i = 2; i < len; ++i)
            {
                ch = path[i];
                curIsSep = ch == '/' || ch == '\\';
                if (curIsSep)
                {
                    if (prevIsSep && buf.Length > 0)
                        buf.Clear();
                    prevIsSep = curIsSep;
                }
                else if (char.IsWhiteSpace(ch))
                {
                    if (prevIsSep)
                        buf.Append(ch);
                    else
                        sb.Append(ch);
                }
                else
                {
                    if (prevIsSep)
                    {
                        sb.Append(Path.DirectorySeparatorChar);
                        prevIsSep = false;
                    }
                    if (buf.Length > 0)
                    {
                        sb.Append(buf.ToString());
                        buf.Clear();
                    }
                    sb.Append(ch);
                }
            }
            if (buf.Length > 0 && prevIsSep)
                sb.Append(Path.DirectorySeparatorChar);
            if (curIsSep && sb[sb.Length - 1] != Path.DirectorySeparatorChar)
                sb.Append(Path.DirectorySeparatorChar);
            var res = sb.ToString();
            var trimmed = res.TrimEnd();
            if (trimmed.Length > 0 && trimmed[trimmed.Length - 1] == Path.VolumeSeparatorChar)
                res = trimmed + Path.DirectorySeparatorChar;
            return res;
        }

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string EnvGetResString1(string key)
            => envGetResourceString1.Value(key);

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string EnvGetResString2(string key, params object[] values)
            => envGetResourceString2.Value(key, values);

        // See https://referencesource.microsoft.com/#mscorlib/system/io/pathinternal.cs,251
        // While paths like "//?/C:/" will work, they're treated the same as "\\.\" paths.
        // Skipping of normalization will *only* occur if back slashes ('\') are used.
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsExtended(string path) => path.Length >= devicePrefixLength
            && IsDirectorySeparatorChar(path[0])
            && (IsDirectorySeparatorChar(path[1]) || path[1] == '?')
            && path[2] == '?'
            && IsDirectorySeparatorChar(path[3]);

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
                isGetTempPath = (cType == tPath && string.Equals("GetTempPath", cMethod?.Name));
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
                    (t == tPath || t == tFile) && fn != null && (fn.StartsWith("System.Web.", StringComparison.Ordinal)/* || (needRemoveLongPrefix = fn.StartsWith("Microsoft.WindowsAPICodePack.", StringComparison.Ordinal))*/))
                );
        }

        [Conditional("TRACE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void TraceGetFullPathInternalPatchedInfo(StackTrace st, Type ct, in bool patched)
        {
            if (ct?.IsSubclassOf(typeof(TraceListener)) ?? false)
                return;

            var sts = st?.ToString() ?? "<no StackTrace>";
            if (sts.IndexOf("System.Web.Hosting.HostingEnvironment.Initialize", StringComparison.Ordinal) > 0 ||
                sts.IndexOf("System.Diagnostics.TraceInternal.", StringComparison.Ordinal) > 0)
                return;

            var ad = AppDomain.CurrentDomain;
            Trace.TraceInformation("{0}[GetFullPathInternal Patched = {1}]:{0}AppDomain = {2} ({3}){0}StackTrace:{0}{4}{0}",
                Environment.NewLine, patched, ad.Id, ad.FriendlyName, sts);
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
    }
}