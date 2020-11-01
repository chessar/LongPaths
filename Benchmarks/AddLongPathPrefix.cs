using BenchmarkDotNet.Attributes;
using System.IO;
using F = System.Func<string, string>;

namespace Chessar.Benchmarks
{
    [BenchmarkCategory(nameof(AddLongPathPrefix))]
    public class AddLongPathPrefix : BenchmarkBase<F>
    {
        private string path;

        protected override void Init()
        {
            path = Path.GetRandomFileName();
            Args = new[] { path };
            Method = typeof(Path).GetMethod("AddLongPathPrefix", privateStatic, null, new[] { typeof(string) }, null);
        }

        [Benchmark(Baseline = true)]
        public string MethodInfoInvoke() => (string)Method.Invoke(null, Args);

        [Benchmark]
        public string CreateDelegate() => Function(path);

        [Benchmark]
        public string ChessarLongPath() => path.AddLongPathPrefix();
    }
}
