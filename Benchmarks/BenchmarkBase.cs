using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Reflection;

namespace Chessar.Benchmarks
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public abstract class BenchmarkBase<T>
        where T : Delegate
    {
        protected const BindingFlags privateStatic
            = BindingFlags.NonPublic | BindingFlags.Static;

#pragma warning disable CA1819 // Properties should not return arrays
        protected object[] Args { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
        protected MethodInfo Method { get; set; }
        protected T Function { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            Init();
            Function = (T)Method.MakeDelegate();
        }

        protected abstract void Init();
    }
}
