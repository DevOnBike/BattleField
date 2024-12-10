using Logic.Pipelining;
using Xunit.Abstractions;

namespace Tests
{
    public class LoggingMiddleware : IMiddleware
    {
        private readonly ITestOutputHelper _output;

        public LoggingMiddleware(ITestOutputHelper output)
        {
            _output = output;
        }

        public async Task ExecuteAsync(MiddlewareContext middlewareContext, MiddlewareDelegate next)
        {
            _output.WriteLine($"Request: {middlewareContext.Request}");
            
            await next(middlewareContext);
            
            _output.WriteLine($"Response: {middlewareContext.Response}");
        }

       
    }

}