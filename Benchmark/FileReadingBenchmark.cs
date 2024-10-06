using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace Benchmark
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    [GcForce]
    [MinIterationCount(3)]
    [MaxIterationCount(4)]
    [InvocationCount(2)]
    public class FileReadingBenchmark
    {
        private string _filePath = @"C:\temp\task_100mb.txt";
        
        [Benchmark(Baseline = true)]
        public void FileReadLines()
        {
            foreach (var line in File.ReadLines(_filePath))
            {
                // do something
            }
            
        }

        [Benchmark]
        public void StreamReaderReadLine()
        {
            using var reader = new StreamReader(_filePath);

            while (reader.ReadLine() is { } line)
            {
                // do something
            }
        }
        
        [Benchmark]
        public async Task StreamReaderReadLineAsync()
        {
            using var reader = new StreamReader(_filePath);

            while (await reader.ReadLineAsync() is { } line)
            {
                // do something
            }
        }
        
        [Benchmark]
        public async Task FileOpenRead()
        {
            const int chunkSize = 1024 * 1024;

            await using var file = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.None, chunkSize);
            
            int bytesRead;
            var buffer = new byte[chunkSize];
            
            while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0)
            {
                var a = bytesRead;
                
                // TODO: Process bytesRead number of bytes from the buffer
                // not the entire buffer as the size of the buffer is 1KB
                // whereas the actual number of bytes that are read are 
                // stored in the bytesRead integer.
            }
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            
        }

       

    }
}