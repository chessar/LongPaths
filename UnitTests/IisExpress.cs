using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Chessar.UnitTests
{
    internal sealed class IisExpress : IDisposable
    {
        private readonly object locker = new object();
        private volatile bool disposed = false;
        private Process process = null;
        private readonly string iisExpressPath = null;

        internal IisExpress() => iisExpressPath =
            Path.Combine(Environment.GetFolderPath(8 == IntPtr.Size
                    ? Environment.SpecialFolder.ProgramFiles
                    : Environment.SpecialFolder.ProgramFilesX86),
                @"IIS Express\iisexpress.exe");

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~IisExpress() => Dispose(false);

        internal void Start(string directoryPath, in int port)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentNullException(nameof(directoryPath));

            var info = new ProcessStartInfo(iisExpressPath)
            {
                WindowStyle = ProcessWindowStyle.Normal,
                ErrorDialog = true,
                LoadUserProfile = true,
                CreateNoWindow = false,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = $"/path:\"{directoryPath.TrimEnd('/', '\\')}\" /port:{port}"
            };

            new Thread(() => StartIisExpress(info))
            {
                IsBackground = true
            }.Start();
        }

        private void Dispose(bool disposing)
        {
            lock (locker)
            {
                if (disposed)
                    return;
                if (disposing)
                    Stop();
                disposed = true;
            }
        }

        internal void Stop()
        {
            if (process != null)
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
                finally
                {
                    process.Dispose();
                    process = null;
                }
            }
        }

        private void StartIisExpress(ProcessStartInfo info)
        {
            try
            {
                Stop();
                Thread.Sleep(100);
                process = Process.Start(info);
                process.WaitForExit();
            }
            catch (Exception)
            {
                Stop();
                throw;
            }
        }
    }
}
