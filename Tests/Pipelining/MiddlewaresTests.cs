using Logic.Pipelining;
using Xunit.Abstractions;

namespace Tests.Pipelining
{
    public class MiddlewaresTests
    {
        [Fact]
        public async Task ExecuteMiddlewares()
        {
            // arrange
            var pipeline = new MiddlewarePipeline();
            
            pipeline.Use(new LoggingMiddleware(_output));
            pipeline.Use(new ResponseMiddleware(_output));

            pipeline.Use(next => async (context) =>
            {
                _output.WriteLine("Middleware 1: Before");
                await next(context);
                _output.WriteLine("Middleware 1: After");
            });
            
            // Create a context
            var context = new MiddlewareContext { Request = "Example Request" };

            // act
            await pipeline.ExecuteAsync(context);

            // assert
            Assert.Equal("Processed by ResponseMiddleware", context.Response);

        }

        private readonly ITestOutputHelper _output;

        public MiddlewaresTests(ITestOutputHelper output)
        {
            _output = output;
        }
    }

}