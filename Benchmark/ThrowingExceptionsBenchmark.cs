using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace Benchmark
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    [GcForce]
    public class ThrowingExceptionsBenchmark
    {
        [Benchmark(Baseline = true)]
        public void ParseWithThrowAllowed()
        {
            try
            {
                int.Parse(_toParse);
            }
            catch
            {
                // pokemon exception handling
            }
        }

        [Benchmark]
        public void ParseWithNoThrow()
        {
            int.TryParse(_toParse, out var a);
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _toParse = Guid.NewGuid().ToString("N");
        }

        private string _toParse;
    }
}