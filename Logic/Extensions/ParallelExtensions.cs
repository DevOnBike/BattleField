namespace Logic.Extensions
{
    public static class ParallelExtensions
    {
        internal static ParallelQuery<TSource> AsForcedParallel<TSource>(this IEnumerable<TSource> source, bool ordered = true)
        {
            var query = source.AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .WithMergeOptions(ParallelMergeOptions.FullyBuffered);

            return ordered ? query : query.AsUnordered();
        }
    }
}