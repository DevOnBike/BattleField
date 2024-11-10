using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace Benchmark
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    [GcForce]
    public class AsyncExecutionBenchmark
    {
        [Benchmark(Baseline = true)]
        public async Task BaselineTask()
        {
            await BaselineAsync(CancellationToken.None);
        }
        
        [Benchmark]
        public async Task DirectTask()
        {
            await DirectTask(CancellationToken.None);
        }
        
        [Benchmark]
        public async ValueTask DirectValueTask()
        {
            await DirectValueTask(CancellationToken.None);
        }
        
        private async Task BaselineAsync(CancellationToken token)
        {
            await InternalAsync(token);
        }
        
        private Task InternalAsync(CancellationToken token)
        {
            return Task.CompletedTask;
        }
        
        private ValueTask InternalValueTaskAsync(CancellationToken token)
        {
            return ValueTask.CompletedTask;
        }
        
        private Task DirectTask(CancellationToken token)
        {
            return InternalAsync(token);
        }
        
        private ValueTask DirectValueTask(CancellationToken token)
        {
            return InternalValueTaskAsync(token);
        }
        

        [GlobalSetup]
        public void GlobalSetup()
        {
            
        }

       

    }
}