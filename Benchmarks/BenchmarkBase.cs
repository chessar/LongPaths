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

        protected object[] args;
        protected MethodInfo method;
        protected T func;

        [GlobalSetup]
        public void Setup()
        {
            Init();
            func = (T)method.MakeDelegate();
        }

        protected abstract void Init();
    }
}
