// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using static Chessar.HookManager;

namespace Chessar
{
    /// <summary>
    /// Hooks for support long paths.
    /// </summary>
    public static partial class Hooks
    {
        #region Fields

        private static readonly BindingFlags
            privateStatic = BindingFlags.NonPublic | BindingFlags.Static;
        private static readonly Lazy<MethodInfo>
            normalizePathOriginal = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("NormalizePath", privateStatic, null,
                    new[] { typeof(string), typeof(bool), typeof(int) }, null)),
            getFullPathInternalOriginal = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("GetFullPathInternal", privateStatic)),
            normalizePath4 = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("NormalizePath", privateStatic, null,
                    new[] { typeof(string), typeof(bool), typeof(int), typeof(bool) }, null)),
            addLongPathPrefix = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("AddLongPathPrefix", privateStatic)),
            removeLongPathPrefix = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("RemoveLongPathPrefix", privateStatic));

        #endregion

        /// <summary>
        /// Patch <see langword="internal static"/> methods
        /// <para>
        /// <see cref="Path"/>.NormalizePath(<see langword="string"/> path,
        /// <see langword="bool"/> fullCheck, <see langword="int"/> maxPathLength)
        /// </para>
        /// for <see cref="FileStream"/>, and
        /// <para>
        /// <see cref="Path"/>.GetFullPathInternal(<see langword="string"/> path)
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
        /// If the <see cref="Path"/>.NormalizePath or <see cref="Path"/>.GetFullPathInternal not found.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the <see cref="Path"/>.NormalizePath or
        /// <see cref="Path"/>.GetFullPathInternal
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
        }

        /// <summary>
        /// Remove long path support patch.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the <see cref="Path"/>.GetFullPathInternal or <see cref="Path"/>.NormalizePath not found.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the <see cref="Path"/>.GetFullPathInternal or <see cref="Path"/>.NormalizePath
        /// method was never hooked.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// If a native call fails. This is unrecoverable.
        /// </exception>
        public static void RemoveLongPathsPatch()
        {
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

            TraceGetFullPathInternalPatchedInfo(st, cType, in needPatch);

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
        private static void TraceGetFullPathInternalPatchedInfo(StackTrace st, Type ct, in bool patched)
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