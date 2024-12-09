using Logic.Pipelining;
using Xunit.Abstractions;

namespace Tests
{
    public class MiddlewaresTests
    {
        [Fact]
        public async Task ExecuteMiddlewares()
        {
            // arrange
            var pipeline = new MiddlewarePipeline();
            
            pipeline.Use(Logger("Middleware 1"));
            pipeline.Use(RequestProcessor());
            pipeline.Use(Logger("Middleware 2"));
            pipeline.Use(Finalizer());

            // Create a context
            var context = new MiddlewareContext { Request = "Example Request" };

            // act
            await pipeline.ExecuteAsync(context);

            // assert
            Assert.True(context.Response == "Response generated!");
            
        }
        
        public MiddlewareDelegate Logger(string name) => async (context, next) =>
        {
            _output.WriteLine($"{name}: Before processing");
            
            await next(); // Call the next middleware
            
            _output.WriteLine($"{name}: After processing");
        };
        
        public MiddlewareDelegate RequestProcessor() => (context, next) =>
        {
            _output.WriteLine("Processing request...");
            _output.WriteLine($"Request Data: {context.Request}");
            
            context.Response = "Response generated!";
            
            return next(); // Call the next middleware
        };

        public MiddlewareDelegate Finalizer() => (context, next) =>
        {
            _output.WriteLine("Final middleware");
            
            return next();
        };

        private readonly ITestOutputHelper _output;

        public MiddlewaresTests(ITestOutputHelper output)
        {
            _output = output;
        }
    }
    
}