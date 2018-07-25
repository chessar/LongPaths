using BenchmarkDotNet.Attributes;
using System;
using System.IO;
using F = System.Func<string, bool>;

namespace Chessar.Benchmarks
{
    [BenchmarkCategory(nameof(IsPathRooted))]
    public class IsPathRooted : BenchmarkBase<F>
    {
        private string path;

        protected override void Init()
        {
            path = Path.GetRandomFileName();
            args = new[] { path };
            method = Type.GetType("System.IO.LongPath").GetMethod("IsPathRooted", privateStatic);
        }

        [Benchmark(Baseline = true)]
        public bool MethodInfoInvoke() => (bool)method.Invoke(null, args);

        [Benchmark]
        public bool CreateDelegate() => func(path);

        [Benchmark]
        public bool ChessarLongPath() => Hooks.IsPathRooted.Value(path);
    }
}
