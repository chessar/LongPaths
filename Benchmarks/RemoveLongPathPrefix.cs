using BenchmarkDotNet.Attributes;
using System.IO;
using F = System.Func<string, string>;

namespace Chessar.Benchmarks
{
    [BenchmarkCategory(nameof(RemoveLongPathPrefix))]
    public class RemoveLongPathPrefix : BenchmarkBase<F>
    {
        private string path;

        protected override void Init()
        {
            path = Path.GetRandomFileName();
            args = new[] { path };
            method = typeof(Path).GetMethod("RemoveLongPathPrefix", privateStatic, null, new[] { typeof(string) }, null);
        }

        [Benchmark(Baseline = true)]
        public string MethodInfoInvoke() => (string)method.Invoke(null, args);

        [Benchmark]
        public string CreateDelegate() => func(path);

        [Benchmark]
        public string ChessarLongPath() => path.AddLongPathPrefix();
    }
}
