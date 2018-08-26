#if NET462
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Chessar.UnitTests
{
    public sealed class TestTraceListener : TraceListener
    {
        private readonly object messageLock = new object();
        private string lastMessage;

        public string LastMessage
        {
            get
            {
                lock (messageLock)
                {
                    return lastMessage;
                }
            }
            set
            {
                lock (messageLock)
                {
                    lastMessage = value;
                }
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Write(string message) => LastMessage = message;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteLine(string message) => Write(message);
    }
}
#endif