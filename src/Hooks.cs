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
using System.Security.AccessControl;
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
                typeof(Path).GetMethod("GetFullPathInternal", privateStatic, null,
                    new[] { typeof(string) }, null)),
            normalizePath4 = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("NormalizePath", privateStatic, null,
                    new[] { typeof(string), typeof(bool), typeof(int), typeof(bool) }, null)),
            addLongPathPrefix = new Lazy<MethodInfo>(() =>
                typeof(Path).GetMethod("AddLongPathPrefix", privateStatic));

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
                typeof(Hooks).GetMethod(nameof(PatchedNormalizePath3), privateStatic, null,
                    new[] { typeof(string), typeof(bool), typeof(int) }, null));

            // patch GetFullPathInternal(string) for IO methods
            try
            {
                Hook(getFullPathInternalOriginal.Value,
                    typeof(Hooks).GetMethod(nameof(PatchedGetFullPathInternal), privateStatic, null,
                        new[] { typeof(string) }, null));
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
        private static string PatchedNormalizePath3(string path, bool fullCheck, int maxPathLength)
        {
            var normalizedPath = NormalizePath4(path, fullCheck, maxPathLength);

            // fix for extend path with long prefix
            if (fullCheck && short.MaxValue == maxPathLength)
                return AddLongPathPrefix(normalizedPath);

            return normalizedPath;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [MethodImpl(MethodImplOptions.NoInlining)] // required
        private static string PatchedGetFullPathInternal(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));
            Contract.EndContractBlock();

            Type callerType = null;
            try
            {
                var st = new StackTrace(1, false);
                callerType = st.GetFrame(0)?.GetMethod()?.DeclaringType;
            }
            catch { }

            //using (var sw = File.AppendText(@"d:\temp\log.txt"))
            //    sw.WriteLine($"{callerType?.Name ?? "(no)"}[{trace.GetFrame(0)?.GetMethod()?.Name ?? "(no)"}]");

            return NeedPatch(callerType)
                ? PatchedNormalizePath3(path, true, short.MaxValue)
                : NormalizePath4(path, true, short.MaxValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool NeedPatch(Type t) => (t != null &&
        (
            // File/Directory/FileInfo/DirectoryInfo
            t == typeof(File) ||
            t == typeof(Directory) ||
            t == typeof(FileInfo) ||
            t == typeof(DirectoryInfo) ||
            t == typeof(FileSystemInfo) ||
            // File/DirectorySecurity
            t == typeof(FileSecurity) ||
            t == typeof(DirectorySecurity) ||
            // Image (FromFile)
            t.FullName == "System.Drawing.Image" ||
            // Directory/DirectoryInfo.GetFiles/GetFolders
            t.Name.StartsWith("FileSystemEnumerableIterator", StringComparison.Ordinal)
        ));

        #endregion
    }
}