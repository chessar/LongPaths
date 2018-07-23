using BenchmarkDotNet.Attributes;
using System.IO;
using System.Reflection;
using F = System.Func<string, string>;

namespace Chessar.Benchmarks
{
    [BenchmarkCategory(nameof(AddLongPathPrefix))]
    public class AddLongPathPrefix
    {
        private string path;
        private object[] args;
        private MethodInfo method;
        private F func1;

        [GlobalSetup]
        public void Setup()
        {
            path = Path.GetRandomFileName();
            args = new[] { path };
            method = typeof(Path).GetMethod("AddLongPathPrefix",
                BindingFlags.NonPublic | BindingFlags.Static,
                null, new[] { typeof(string) }, null);
            func1 = (F)method.MakeDelegate();
        }

        [Benchmark, BenchmarkCategory("MethodInfoInvoke")]
        public string AddLongPathPrefix_MethodInfoInvoke() => (string)method.Invoke(null, args);

        [Benchmark, BenchmarkCategory("CreateDelegate")]
        public string AddLongPathPrefix_CreateDelegate() => func1(path);

        [Benchmark, BenchmarkCategory("ChessarLongPath")]
        public string AddLongPathPrefix_ChessarLongPath() => path.AddLongPathPrefix();
    }
}
