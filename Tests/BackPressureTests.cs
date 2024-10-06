using System.Reactive.Linq;
using System.Threading.Tasks.Dataflow;
using Akka.Actor;
using Akka.Streams;
using Akka.Streams.Dsl;
using Xunit.Abstractions;

namespace Tests
{
    public class BackPressureTests
    {
        [Fact(Skip = "ada")]
        public void UsingReactive()
        {
            // Simulate a fast producer
            var fastProducer = Observable.Interval(TimeSpan.FromMilliseconds(100));

            // Buffer incoming events into batches of 5 items
            var bufferedObservable = fastProducer.Buffer(TimeSpan.FromSeconds(1), 5);

            // Slow consumer processing
            bufferedObservable.Subscribe(batch =>
            {
                _output.WriteLine($"Processing batch of {batch.Count} items");
                Thread.Sleep(2000); // Simulate slow processing
            });
            
            
        }
        
        [Fact]
        public async Task UsingDataflowBlocks()
        {
            // Producer block with limited capacity
            var bufferBlock = new BufferBlock<int>(new DataflowBlockOptions
            {
                BoundedCapacity = 10 // Backpressure limit
            });

            // Consumer block
            var actionBlock = new ActionBlock<int>(async item =>
            {
                _output.WriteLine($"Processing item {item}");
                await Task.Delay(500); // Simulate slow consumer processing
            });

            // Link the blocks
            bufferBlock.LinkTo(actionBlock, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            // Producer generates data
            for (var i = 0; i < 100; i++)
            {
                await bufferBlock.SendAsync(i);
                _output.WriteLine($"Produced item {i}");
            }

            bufferBlock.Complete();
            await actionBlock.Completion;
        }

        [Fact]
        public async Task UsingAkkaNet()
        {
            // Create the actor system
            using var actorSystem = ActorSystem.Create("MyActorSystem");

            // Create a fast source (simulating a fast producer)
            var producer = Source.From(Enumerable.Range(1, 100)).Async(); 
            
            // Add a buffer of 10 elements, dropping the oldest element when full
            producer = producer.Buffer(10, OverflowStrategy.Backpressure);

            // Create a slow consumer (simulating a slow consumer)
            var consumer = Sink.ForEachAsync<int>(2, async msg =>
            {
                // Simulate slow processing (500ms delay per message)
                _output.WriteLine($"Processing {msg} @ {DateTime.Now:hh:mm:fff}");
                
                await Task.Delay(500); // Slow processing  
            });

            // Run the stream
            await producer.RunWith(consumer, actorSystem);

            _output.WriteLine("stream completed");
            
            // Shutdown the actor system after you're done
            await actorSystem.Terminate();
        }
        
        private readonly ITestOutputHelper _output;

        public BackPressureTests(ITestOutputHelper output)
        {
            _output = output;
        }
    }
}