using BenchmarkDotNet.Running;
using System;
using System.Linq;
using System.Reflection;

namespace Chessar.Benchmarks
{
    static class Program
    {
        static void Main()
        {
            BenchmarkSwitcher.FromTypes(Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOfRawGeneric(typeof(BenchmarkBase<>))).ToArray()).RunAllJoined();
            Console.ReadKey();
        }
    }
}
