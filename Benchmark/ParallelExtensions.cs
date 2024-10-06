namespace Benchmark
{
    public static class ParallelExtensions
    {
        public static ParallelQuery<TSource> AsForcedParallel<TSource>(this IEnumerable<TSource> source, bool ordered = true)
        {
            var query = source.AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .WithMergeOptions(ParallelMergeOptions.FullyBuffered);

            return ordered ? query : query.AsUnordered();
        }
    }
}