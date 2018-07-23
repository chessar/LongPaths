using BenchmarkDotNet.Running;
using System;

namespace Chessar.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<AddLongPathPrefix>();
            Console.ReadKey();
        }
    }
}
