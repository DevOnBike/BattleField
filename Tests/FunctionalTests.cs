using Xunit.Abstractions;

namespace Tests
{
    public class FunctionalTests
    {
        [Fact]
        public async Task MergeTest1()
        {
            
        }


        private static readonly char[] _allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        
        private static readonly ParallelOptions _parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };
        
        private readonly ITestOutputHelper _output;

        public FunctionalTests(ITestOutputHelper output)
        {
            _output = output;
        }
    }
}