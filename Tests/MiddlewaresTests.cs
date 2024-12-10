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
            
            pipeline.Use(new LoggingMiddleware(_output));
            pipeline.Use(new ResponseMiddleware());
/*
            pipeline.Use((i) => async (context) =>
            {
                _output.WriteLine("Middleware 1: Before");
                await next(context);
                _output.WriteLine("Middleware 1: After");
            });
            
            pipeline.Use((i) => async (context, next) =>
            {
                _output.WriteLine("Middleware 2: Before");
                context.Response = "Handled by Middleware 2";
                
                await next;
                
                _output.WriteLine("Middleware 2: After");
            });
*/
            // Create a context
            var context = new MiddlewareContext { Request = "Example Request" };

            // act
            await pipeline.ExecuteAsync(context);

            // assert
            Assert.True(context.Response == "Processed by ResponseMiddleware");

        }

        private readonly ITestOutputHelper _output;

        public MiddlewaresTests(ITestOutputHelper output)
        {
            _output = output;
        }
    }

}