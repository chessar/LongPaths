// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.IO;
using System.Text;
using static Chessar.Hooks;
using static System.Console;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            if (!AddSupportLongPaths())
                return;

            string workFolder = null;

            try
            {
                var currentFolder = Directory.GetCurrentDirectory();
                workFolder = Path.Combine(currentFolder, Guid.NewGuid().ToString());
                WriteLine($"Work folder:");
                WriteLine(workFolder);

                var longPathFolder = GetLongPath(workFolder);
                WriteLine($"Length of long folder path: {longPathFolder.Length}");

                WriteLine("Creating folder with long path...");
                var dirInfo = Directory.CreateDirectory(longPathFolder);
                WriteLine($"Folder with long path '{DisplayLongPath(dirInfo.FullName)}' created");

                WriteLine("Creating file...");
                var filePath = Path.Combine(longPathFolder, "file.txt");
                var fileInfo = new FileInfo(filePath);
                fileInfo.CreateText().Close();
                fileInfo.Refresh();
                WriteLine($"File '{DisplayLongPath(fileInfo.FullName)}' created");

                Write("Write to file... ");
                using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    var bytes = Encoding.UTF8.GetBytes("Content of file with long path");
                    fs.Write(bytes, 0, bytes.Length);
                }
                WriteLine("Success");

                WriteLine("Read from file... ");
                var fileContent = File.ReadAllText(filePath);
                WriteLine("Success. File content:");
                WriteLine(fileContent);
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }
            finally
            {
                try
                {
                    if (Directory.Exists(workFolder))
                        Directory.Delete(workFolder, true);
                }
                catch { }

                RemoveSupportLongPaths();
            }
        }

        static bool AddSupportLongPaths()
        {
            try
            {
                PatchLongPaths();
                return true;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
                ReadKey();
                return false;
            }
        }

        static void RemoveSupportLongPaths()
        {
            try
            {
                RemoveLongPathsPatch();
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            ReadKey();
        }

        static string GetLongPath(string basePath)
        {
            var sb = new StringBuilder(basePath.TrimEnd('/', '\\'), short.MaxValue);
            var longName = $"{Path.DirectorySeparatorChar}{new string('a', 254)}"; // see MaxComponentLength https://referencesource.microsoft.com/#mscorlib/system/io/pathinternal.cs,30
            var parts = (short.MaxValue - sb.Length) / longName.Length - 1;
            for (int i = 0; i < parts; ++i)
                sb.Append($"{longName}");
            return sb.ToString();
        }

        static string DisplayLongPath(string path) =>
            $"{path.Remove(10)} ... {path.Substring(path.Length - 10)}";
    }
}
