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
using System.Web;
using System.Web.Hosting;
using static Chessar.HookManager;

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

        #endregion

        #region Fields

        private static readonly BindingFlags
            privateStatic = BindingFlags.NonPublic | BindingFlags.Static,
            privateInstance = BindingFlags.NonPublic | BindingFlags.Instance;
        private static readonly Lazy<MethodInfo>
            normalizePathOriginal = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("NormalizePath", privateStatic, null,
                    new[] { typeof(string), typeof(bool), typeof(int) }, null)),
            getFullPathInternalOriginal = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("GetFullPathInternal", privateStatic)),
            normalizePath4 = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("NormalizePath", privateStatic, null,
                    new[] { typeof(string), typeof(bool), typeof(int), typeof(bool) }, null)),
            directoryInternalMove = new Lazy<MethodInfo>(() =>
                typeof(Directory).GetMethod("InternalMove", privateStatic, null,
                    new[] { typeof(string), typeof(string), typeof(bool) }, null)),
            longPathDirectoryMove = new Lazy<MethodInfo>(() =>
                Type.GetType("System.IO.LongPathDirectory").GetMethod("Move", privateStatic, null,
                    new[] { typeof(string), typeof(string) }, null)),
            addLongPathPrefix = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("AddLongPathPrefix", privateStatic)),
            removeLongPathPrefix = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("RemoveLongPathPrefix", privateStatic, null,
                    new[] { typeof(string) }, null)),
            nosCreateInternal = new Lazy<MethodInfo>(() =>
                typeof(NativeObjectSecurity).GetMethod("CreateInternal", privateStatic)),
            envGetResourceString1 = new Lazy<MethodInfo>(() =>
                typeof(Environment).GetMethod("GetResourceString", privateStatic, null,
                    new[] { typeof(string) }, null)),
            envGetResourceString2 = new Lazy<MethodInfo>(() =>
                typeof(Environment).GetMethod("GetResourceString", privateStatic, null,
                    new[] { typeof(string), typeof(object[]) }, null)),
            win32GetSecurityInfo = new Lazy<MethodInfo>(() =>
                Type.GetType("System.Security.AccessControl.Win32").GetMethod("GetSecurityInfo", privateStatic)),
            isPathRooted = new Lazy<MethodInfo>(() =>
                Type.GetType("System.IO.LongPath").GetMethod("IsPathRooted", privateStatic)),
            responseGetNormalizedFilename = new Lazy<MethodInfo>(() =>
                typeof(HttpResponse).GetMethod("GetNormalizedFilename", privateInstance)),
            uriCreateThis = new Lazy<MethodInfo>(() =>
                typeof(Uri).GetMethod("CreateThis", privateInstance)),
            uriParseScheme = new Lazy<MethodInfo>(() =>
                typeof(Uri).GetMethod("ParseScheme", privateStatic)),
            uriInitializeUri = new Lazy<MethodInfo>(() =>
                typeof(Uri).GetMethod("InitializeUri", privateInstance));
        private static Lazy<ConstructorInfo> csdCtor = new Lazy<ConstructorInfo>(() =>
            (typeof(CommonSecurityDescriptor)).GetConstructors(privateInstance)
            ?.FirstOrDefault(ci => ci.GetParameters().Length == 4));
        private static Lazy<FieldInfo>
            mStringUriFld = new Lazy<FieldInfo>(() => typeof(Uri).GetField("m_String", privateInstance)),
            mFlagsUriFld = new Lazy<FieldInfo>(() => typeof(Uri).GetField("m_Flags", privateInstance)),
            mSyntaxUriFld = new Lazy<FieldInfo>(() => typeof(Uri).GetField("m_Syntax", privateInstance));

        #endregion

        /// <summary>
        /// Patch <see langword="internal static"/> methods
        /// <para>
        /// <see cref="Path"/>.NormalizePath(<see langword="string"/> path,
        /// <see langword="bool"/> fullCheck, <see langword="int"/> maxPathLength)
        /// </para>
        /// for <see cref="FileStream"/>, and
        /// <para>
        /// <see cref="Path"/>.GetFullPathInternal(<see langword="string"/> path),
        /// <see cref="Directory"/>.InternalMove(<see langword="string"/> sourceDirName, <see langword="string"/> destDirName, <see langword="bool"/> checkHost),
        /// <see cref="NativeObjectSecurity"/>.CreateInternal(...),
        /// <see cref="HttpResponse"/>.GetNormalizedFilename(<see langword="string"/> fn),
        /// <see cref="Uri"/>.CreateThis(<see langword="string"/> uri, <see langword="bool"/> dontEscape, <see cref="UriKind"/> uriKind)
        /// </para><para>
        /// for methods from <see cref="File"/>, <see cref="FileInfo"/>,
        /// <see cref="Directory"/>, <see cref="DirectoryInfo"/>, etc..
        /// </para>
        /// using JMP hook for support long paths.
        /// <para>
        /// Patched method adding long path prefix <see langword="\\?\"/>
        /// (or <see langword="\\?\UNC\"/> for network shares)
        /// if <see langword="fullCheck"/>=<see langword="true"/> and
        /// <see langword="maxPathLength"/>=<see cref="short.MaxValue"/>.
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the <see cref="Path"/>.NormalizePath,
        /// <see cref="Path"/>.GetFullPathInternal,
        /// <see cref="Directory"/>.InternalMove,
        /// <see cref="NativeObjectSecurity"/>.CreateInternal,
        /// <see cref="HttpResponse"/>.GetNormalizedFilename or
        /// <see cref="Uri"/>.CreateThis
        /// not found.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the <see cref="Path"/>.NormalizePath,
        /// <see cref="Path"/>.GetFullPathInternal,
        /// <see cref="Directory"/>.InternalMove,
        /// <see cref="NativeObjectSecurity"/>.CreateInternal,
        /// <see cref="HttpResponse"/>.GetNormalizedFilename or
        /// <see cref="Uri"/>.CreateThis
        /// is already hooked.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// If a native call fails. This is unrecoverable.
        /// </exception>
        public static void PatchLongPaths()
        {
            // patch NormalizePath(string, bool, int) for FileStream
            Hook(normalizePathOriginal.Value,
                typeof(Hooks).GetMethod(nameof(NormalizePathPatched), privateStatic));

            // patch GetFullPathInternal(string) for IO methods
            try
            {
                Hook(getFullPathInternalOriginal.Value,
                    typeof(Hooks).GetMethod(nameof(GetFullPathInternalPatched), privateStatic));
            }
            catch
            {
                Unhook(normalizePathOriginal.Value);
                throw;
            }

            // patch Directory.InternalMove(string, string, bool)
            try
            {
                if (longPathDirectoryMove.Value != null)
                    Hook(directoryInternalMove.Value,
                        typeof(Hooks).GetMethod(nameof(DirectoryMovePatched), privateStatic));
            }
            catch
            {
                Unhook(getFullPathInternalOriginal.Value);
                Unhook(normalizePathOriginal.Value);
                throw;
            }

            // patch NativeObjectSecurity.CreateInternal
            try
            {
                if (nosCreateInternal.Value != null)
                    Hook(nosCreateInternal.Value,
                        typeof(Hooks).GetMethod(nameof(NosCreateInternalPatched), privateStatic));
            }
            catch
            {
                if (longPathDirectoryMove.Value != null)
                    Unhook(directoryInternalMove.Value);
                Unhook(getFullPathInternalOriginal.Value);
                Unhook(normalizePathOriginal.Value);
                throw;
            }

            // patch HttpResponse.GetNormalizedFilename
            try
            {
                Hook(responseGetNormalizedFilename.Value,
                    typeof(Hooks).GetMethod(nameof(GetNormalizedFilenamePatched), privateStatic));
            }
            catch
            {
                if (nosCreateInternal.Value != null)
                    Unhook(nosCreateInternal.Value);
                if (longPathDirectoryMove.Value != null)
                    Unhook(directoryInternalMove.Value);
                Unhook(getFullPathInternalOriginal.Value);
                Unhook(normalizePathOriginal.Value);
                throw;
            }

            // patch Uri.CreateThis(string, bool, UriKind)
            try
            {
                Hook(uriCreateThis.Value,
                    typeof(Hooks).GetMethod(nameof(UriCreateThisPatched), privateStatic));
            }
            catch
            {
                Unhook(responseGetNormalizedFilename.Value);
                if (nosCreateInternal.Value != null)
                    Unhook(nosCreateInternal.Value);
                if (longPathDirectoryMove.Value != null)
                    Unhook(directoryInternalMove.Value);
                Unhook(getFullPathInternalOriginal.Value);
                Unhook(normalizePathOriginal.Value);
                throw;
            }
        }

        /// <summary>
        /// Remove long path support patch.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the <see cref="Path"/>.GetFullPathInternal,
        /// <see cref="Path"/>.NormalizePath,
        /// <see cref="Directory"/>.InternalMove,
        /// <see cref="NativeObjectSecurity"/>.CreateInternal,
        /// <see cref="HttpResponse"/>.GetNormalizedFilename or
        /// <see cref="Uri"/>.CreateThis
        /// not found.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the <see cref="Path"/>.GetFullPathInternal,
        /// <see cref="Path"/>.NormalizePath,
        /// <see cref="Directory"/>.InternalMove,
        /// <see cref="NativeObjectSecurity"/>.CreateInternal,
        /// <see cref="HttpResponse"/>.GetNormalizedFilename or
        /// <see cref="Uri"/>.CreateThis
        /// method was never hooked.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// If a native call fails. This is unrecoverable.
        /// </exception>
        public static void RemoveLongPathsPatch()
        {
            Unhook(uriCreateThis.Value);
            Unhook(responseGetNormalizedFilename.Value);
            if (nosCreateInternal.Value != null)
                Unhook(nosCreateInternal.Value);
            if (longPathDirectoryMove.Value != null)
                Unhook(directoryInternalMove.Value);
            Unhook(getFullPathInternalOriginal.Value);
            Unhook(normalizePathOriginal.Value);
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
                return (string)addLongPathPrefix.Value?.Invoke(null, new object[] { path }) ?? path;
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
                return (string)removeLongPathPrefix.Value?.Invoke(null, new object[] { path }) ?? path;
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #region Private

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void UriCreateThisPatched(Uri thisUri, string uri, bool dontEscape, UriKind uriKind)
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

                args = new [] { err, uriKind, null };
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

        [Pure, MethodImpl(MethodImplOptions.NoInlining)] // required
        private static string GetNormalizedFilenamePatched(HttpResponse response, string fn)
        {
            try
            {
                // If it's not a physical path, call MapPath on it
                if (!(bool)isPathRooted.Value.Invoke(null, new object[] { fn }))
                {
                    var fldInfo = typeof(HttpResponse).GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance);
                    var context = fldInfo.GetValue(response) as HttpContext;
                    var request = context?.Request;

                    if (request != null)
                        fn = request.MapPath(fn); // relative to current request
                    else
                        fn = HostingEnvironment.MapPath(fn);
                }

                return fn.AddLongPathPrefix();
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        private delegate Exception ExceptionFromErrorCode(int errorCode, string name, SafeHandle handle, object context);

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string EnvGetResString(string key) => (string)envGetResourceString1
            .Value.Invoke(null, new object[] { key });

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string EnvGetResString(string key, params object[] values) => (string)envGetResourceString2
            .Value.Invoke(null, new object[] { key, values });

        [MethodImpl(MethodImplOptions.NoInlining)] // required
        private static CommonSecurityDescriptor NosCreateInternalPatched(ResourceType resourceType,
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
                object[] parameters = { resourceType, name?.AddLongPathPrefix(), handle, includeSections, null };
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
                            exception = new InvalidOperationException(EnvGetResString("AccessControl_InvalidOwner"));
                        else if (error == ERROR_INVALID_PRIMARY_GROUP)
                            exception = new InvalidOperationException(EnvGetResString("AccessControl_InvalidGroup"));
                        else if (error == ERROR_INVALID_PARAMETER)
                            exception = new InvalidOperationException(EnvGetResString("AccessControl_UnexpectedError", error));
                        else if (error == ERROR_INVALID_NAME)
                            exception = new ArgumentException(EnvGetResString("Argument_InvalidName"), nameof(name));
                        else if (error == ERROR_FILE_NOT_FOUND)
                            exception = (name is null ? new FileNotFoundException() : new FileNotFoundException(name));
                        else if (error == ERROR_NO_SECURITY_ON_OBJECT)
                            exception = new NotSupportedException(EnvGetResString("AccessControl_NoAssociatedSecurity"));
                        else
                        {
                            Contract.Assert(false, string.Format(CultureInfo.InvariantCulture, "Win32GetSecurityInfo() failed with unexpected error code {0}", error));
                            exception = new InvalidOperationException(EnvGetResString("AccessControl_UnexpectedError", error));
                        }
                    }

                    throw exception;
                }

                var rawSD = (RawSecurityDescriptor)parameters[parameters.Length - 1];
                return (CommonSecurityDescriptor)csdCtor.Value
                    ?.Invoke(new object[] { isContainer, false, rawSD, true });
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string NormalizePath4(string path, bool fullCheck, int maxPathLength)
        {
            try
            {
                return (string)normalizePath4.Value.Invoke(null,
                    new object[] { path, fullCheck, maxPathLength, true });
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)] // required
        private static string NormalizePathPatched(string path, bool fullCheck, int maxPathLength)
        {
            var normalizedPath = NormalizePath4(path, fullCheck, maxPathLength);

            // fix for extend path with long prefix
            if (fullCheck && short.MaxValue == maxPathLength)
                return AddLongPathPrefix(normalizedPath);

            return normalizedPath;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        [MethodImpl(MethodImplOptions.NoInlining)] // required
        private static void DirectoryMovePatched(string sourceDirName, string destDirName, bool checkHost)
        {
            try
            {
                longPathDirectoryMove.Value.Invoke(null,
                    new object[] { sourceDirName, destDirName });
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [MethodImpl(MethodImplOptions.NoInlining)] // required
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

            return needPatch
                ? NormalizePathPatched(path, true, short.MaxValue)
                : NormalizePath4(path, true, short.MaxValue);
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
            t == typeof(Path) && fn != null && (
                fn.StartsWith("System.Web.", StringComparison.Ordinal) ||
                fn.StartsWith("System.CodeDom.", StringComparison.Ordinal) ||
                fn.StartsWith("System.Drawing.IntSecurity", StringComparison.Ordinal)))/*||

            // AppDomainSetup (https://referencesource.microsoft.com/#mscorlib/system/AppDomainSetup.cs,881)
            t == typeof(AppDomainSetup) ||

            // ConfigTreeParser (https://referencesource.microsoft.com/#mscorlib/system/cfgparser.cs,274)
            string.Equals(t.Name, "ConfigTreeParser")*/
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
    }
}