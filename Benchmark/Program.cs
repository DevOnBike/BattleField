﻿using BenchmarkDotNet.Running;

namespace Benchmark
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var summaries = BenchmarkRunner.Run<ThrowingExceptionsBenchmark>();

        }
    }
}