// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace Chessar
{
    /// <summary>
    /// Hook Manager.
    /// <para>
    /// Used to hook and unhook all hooked functions in a process.
    /// No method in this class is thread safe and should
    /// be shared among all threads (protected by a Lock).
    /// Programs will most likely crash if a hook is installed
    /// while another thread is calling the target function.
    /// </para>
    /// <para>
    /// See <see href="https://github.com/wledfor2/PlayHooky/blob/master/HookManager.cs"/>.
    /// </para>
    /// </summary>
    // TODO: add lock/mutex for tread safe
    internal static class HookManager
    {
        #region Fields

        private static readonly bool is64 = IntPtr.Size == 8;
        private static readonly ConcurrentDictionary<MethodInfo, byte[]> hooks
            = new ConcurrentDictionary<MethodInfo, byte[]>();

        #endregion

        #region Methods

        /// <summary>
        /// Replaces the method call <paramref name="original"/> with
        /// the method <paramref name="replacement"/> with a standard x86/x64 JMP hook.
        /// <para>
        /// The methods do not have to be in the same assembly.
        /// </para><para>
        /// Original may be static, but <paramref name="replacement"/>
        /// <see langword="MUST"/> be static and accept the same arguments
        /// (if the hooked method is non-static, the first argument should be of the <see langword="class"/> type).
        /// </para>
        /// </summary>
        /// <param name="original"><see cref="MethodInfo"/> for the function to be hooked.</param>
        /// <param name="replacement"><see cref="MethodInfo"/> for the function to replace the <paramref name="original"/> function.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="original"/> or <paramref name="replacement"/> are <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="original"/> and <paramref name="replacement"/> are the same function,
        /// <paramref name="original"/> is generic, <paramref name="replacement"/> is generic or non-static,
        /// or if the target function is already hooked.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// If a native call fails. This is unrecoverable.
        /// </exception>
        [HandleProcessCorruptedStateExceptions]
        internal static unsafe void Hook(MethodInfo original, MethodInfo replacement)
        {
            if (original is null)
                throw new ArgumentNullException(nameof(original));
            if (replacement is null)
                throw new ArgumentNullException(nameof(replacement));
            if (original == replacement)
                throw new ArgumentException("A function {0} can't hook itself.".Format(original));
            if (original.IsGenericMethod)
                throw new ArgumentException("Original method {0} cannot be generic.".Format(original), nameof(original));
            if (replacement.IsGenericMethod || !replacement.IsStatic)
                throw new ArgumentException("Hook method {0} must be static and non-generic.".Format(replacement), nameof(replacement));
            if (hooks.ContainsKey(original))
                throw new ArgumentException("Attempting to hook an already hooked method {0}".Format(replacement), nameof(original));
            Contract.EndContractBlock();

            hooks.TryAdd(original, PatchJMP(original, replacement));
        }

        [HandleProcessCorruptedStateExceptions]
        internal static Exception BatchUnhook(params MethodInfo[] methods)
        {
            if (methods is null || methods.Length == 0)
                return null;
            var errors = new List<Exception>(methods.Length);
            for (int i = methods.Length - 1; i >= 0; i--)
            {
                var m = methods[i];
                if (m is null)
                    continue;
                if (!hooks.TryGetValue(m, out byte[] originalOpcodes))
                {
                    var ex = new ArgumentException("Method {0} was never hooked".Format(m));
                    errors.Add(ex);
                    TraceException(ex);
                    continue;
                }
                try
                {
                    UnhookJMP(m, originalOpcodes);
                }
#pragma warning disable CA2153 // Do Not Catch Corrupted State Exceptions
                catch (Exception ex)
#pragma warning restore CA2153 // Do Not Catch Corrupted State Exceptions
                {
                    errors.Add(ex);
                    TraceException(ex);
                    continue;
                }
                hooks.TryRemove(m, out _);
            }
            return (errors.Count > 1) ? new AggregateException(errors.ToArray())
                : errors.Count == 1 ? errors[0] : null;
        }

        [Conditional("TRACE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void TraceException(Exception ex)
        {
            Trace.TraceError(ex.ToString());
            Trace.Flush();
        }

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string Format(this string format, MethodInfo method) => string.Format(CultureInfo.InvariantCulture,
            format, string.Format(CultureInfo.InvariantCulture, "[{0}.{1}]", method?.DeclaringType
                .FullName ?? "<unknown type>", method?.Name ?? "<unknown name>"));

        [HandleProcessCorruptedStateExceptions]
        private static unsafe byte[] PatchJMP(MethodInfo original, MethodInfo replacement)
        {
            //JIT compile methods
            RuntimeHelpers.PrepareMethod(original.MethodHandle);
            RuntimeHelpers.PrepareMethod(replacement.MethodHandle);
            // compile both functions and get pointers to them.
            var originalSite = original.MethodHandle.GetFunctionPointer();
            var replacementSite = replacement.MethodHandle.GetFunctionPointer();
            // instruction opcodes are 13 bytes on 64-bit, 6 bytes on 32-bit
            var offset = (is64 ? 13u : 6u);
            // we store the original opcodes for restoration later
            var originalOpcodes = new byte[offset];

            unsafe
            {
                // segfault protection
                var oldProtecton = VirtualProtect(originalSite,
                    (uint)originalOpcodes.Length, (uint)NativeMethods.PageProtection.PAGE_EXECUTE_READWRITE);

                // get unmanaged function pointer to address of original site
                var originalSitePointer = (byte*)originalSite.ToPointer();

                // copy the original opcodes
                for (int k = 0; k < offset; k++)
                    originalOpcodes[k] = *(originalSitePointer + k);

                // check which architecture we are patching for
                if (is64)
                {
                    // mov r11, replacementSite
                    *originalSitePointer = 0x49;
                    *(originalSitePointer + 1) = 0xBB;
                    *((ulong*)(originalSitePointer + 2)) = (ulong)replacementSite.ToInt64(); // sets 8 bytes
                    // jmp r11
                    *(originalSitePointer + 10) = 0x41;
                    *(originalSitePointer + 11) = 0xFF;
                    *(originalSitePointer + 12) = 0xE3;
                }
                else
                {
                    // push replacementSite
                    *originalSitePointer = 0x68;
                    *((uint*)(originalSitePointer + 1)) = (uint)replacementSite.ToInt32(); // sets 4 bytes
                    // ret
                    *(originalSitePointer + 5) = 0xC3;
                }

                // flush insutruction cache to make sure our new code executes
                FlushInstructionCache(originalSite, (uint)originalOpcodes.Length);

                // done
                VirtualProtect(originalSite, (uint)originalOpcodes.Length, oldProtecton);
            }

            // return original opcodes
            return originalOpcodes;
        }

        [HandleProcessCorruptedStateExceptions]
        private static unsafe void UnhookJMP(MethodInfo original, byte[] originalOpcodes)
        {
            var originalSite = original.MethodHandle.GetFunctionPointer();

            unsafe
            {
                // segfault protection
                var oldProtecton = VirtualProtect(originalSite, (uint)originalOpcodes.Length, (uint)NativeMethods.PageProtection.PAGE_EXECUTE_READWRITE);

                // get unmanaged function pointer to address of original site
                var originalSitePointer = (byte*)originalSite.ToPointer();

                // put the original bytes back where they belong
                for (int k = 0; k < originalOpcodes.Length; k++)
                    // restore current opcode to former value
                    *(originalSitePointer + k) = originalOpcodes[k];

                // flush insutruction cache to make sure our new code executes
                FlushInstructionCache(originalSite, (uint)originalOpcodes.Length);

                // done
                VirtualProtect(originalSite, (uint)originalOpcodes.Length, oldProtecton);
            }
        }

        [HandleProcessCorruptedStateExceptions]
        private static uint VirtualProtect(IntPtr address, uint size, uint protectionFlags)
        {
            if (!NativeMethods.VirtualProtect(address, (UIntPtr)size, protectionFlags, out uint oldProtection))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return oldProtection;
        }

        [HandleProcessCorruptedStateExceptions]
        private static void FlushInstructionCache(IntPtr address, uint size) =>
            NativeMethods.FlushInstructionCache(NativeMethods.GetCurrentProcess(), address, (UIntPtr)size);

        #endregion

        #region Natives

        private static class NativeMethods
        {
            private const string kernel32_dll = "kernel32.dll";
            [DllImport(kernel32_dll, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            [HandleProcessCorruptedStateExceptions]
            internal static extern bool FlushInstructionCache(IntPtr hProcess, IntPtr lpBaseAddress, UIntPtr dwSize);
            [DllImport(kernel32_dll, SetLastError = true)]
            [HandleProcessCorruptedStateExceptions]
            internal static extern IntPtr GetCurrentProcess();
            [DllImport(kernel32_dll, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            [HandleProcessCorruptedStateExceptions]
            internal static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
            [Flags]
            internal enum PageProtection : uint
            {
                PAGE_NOACCESS = 0x01,
                PAGE_READONLY = 0x02,
                PAGE_READWRITE = 0x04,
                PAGE_WRITECOPY = 0x08,
                PAGE_EXECUTE = 0x10,
                PAGE_EXECUTE_READ = 0x20,
                PAGE_EXECUTE_READWRITE = 0x40,
                PAGE_EXECUTE_WRITECOPY = 0x80,
                PAGE_GUARD = 0x100,
                PAGE_NOCACHE = 0x200,
                PAGE_WRITECOMBINE = 0x400
            }
        }

        #endregion
    }
}