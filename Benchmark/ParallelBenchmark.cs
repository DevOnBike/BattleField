using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace Benchmark
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    [GcForce]
    public class ParallelBenchmark
    {
        private readonly List<Guid> _list = new List<Guid>();
        private readonly Guid _toFind = Guid.NewGuid();

        private static readonly ParallelOptions _parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        [Benchmark(Baseline = true)]
        public void SimpleLinq()
        {
            var find = _list.FirstOrDefault(x => x == _toFind);
        }

        [Benchmark]
        public void SimpleForEach()
        {
            var found = SearchForEach(_toFind, _list);
        }
        
        [Benchmark]
        public void AsCustomForcedParallel()
        {
            var find = _list.AsForcedParallel().FirstOrDefault(x => x == _toFind);
        }


        [Benchmark]
        public void AsCustomForcedParallelUnOrdered()
        {
            var find = _list.AsForcedParallel(false).FirstOrDefault(x => x == _toFind);
        }

        [Benchmark]
        public void AsParallel()
        {
            var find = _list.AsParallel().FirstOrDefault(x => x == _toFind);
        }

        [Benchmark]
        public void AsParallelOrdered()
        {
            var find = _list.AsParallel().AsOrdered().FirstOrDefault(x => x == _toFind);
        }

        [Benchmark]
        public void ParallelFor()
        {
            var idx = -1;

            Parallel.For(0, _list.Count - 1, _parallelOptions, (i, state) =>
            {
                if (_list[i] == _toFind && idx < 0)
                {
                    Interlocked.Exchange(ref idx, i);
                    state.Stop();
                }
            });

        }

        [Benchmark]
        public void ParallelForEachWithStaticPartitioner()
        {
            var idx = -1;

            Parallel.ForEach(Partitioner.Create(0, _list.Count), _parallelOptions, (range, state) =>
            {
                for (var i = range.Item1; i < range.Item2; i++)
                {
                    if (_list[i] == _toFind && idx < 0)
                    {
                        state.Stop();
                        Interlocked.Exchange(ref idx, i);
                    }
                }
            });
            
        }

        [Benchmark]
        public void ParallelForEachWithStaticChunkPartitioner()
        {
            var idx = -1;
            var chunkSize = 1000;
            
            Parallel.ForEach(Partitioner.Create(0, _list.Count, chunkSize), _parallelOptions, (range, state) =>
            {
                for (var i = range.Item1; i < range.Item2; i++)
                {
                    if (_list[i] == _toFind && idx < 0)
                    {
                        Interlocked.Exchange(ref idx, i);
                        state.Stop();
                    }
                }
            });

        }

        [Benchmark]
        public void ParallelForEachWithLoadBalancingPartitioner()
        {
            Parallel.ForEach(Partitioner.Create(_list, true), _parallelOptions, (item, state) =>
            {
                if (item == _toFind)
                {
                    state.Stop();
                }
            });

        }

        [Benchmark]
        public void ParallelForEach()
        {
            Guid found;

            Parallel.ForEach(_list, _parallelOptions, (item, state, i) =>
            {
                if (item == _toFind)
                {
                    found = item;
                    state.Break();
                }
            });

        }

        [Benchmark]
        public void ParallelForAll()
        {
            Guid found;

            _list.AsForcedParallel().ForAll(item =>
            {
                if (item == _toFind)
                {
                    found = item;
                }
            });

        }


        [GlobalSetup]
        public void GlobalSetup()
        {
            _list.Clear();

            for (var i = 0; i < 1_000_000 - 1; i++)
            {
                _list.Add(Guid.NewGuid());
            }

            _list.Add(_toFind);
        }

        private static bool SearchForEach(Guid toFind, List<Guid> list)
        {
            foreach (var guid in list)
            {
                if (guid == toFind)
                {
                    return true;
                }
            }

            return false;
        }

    }
}