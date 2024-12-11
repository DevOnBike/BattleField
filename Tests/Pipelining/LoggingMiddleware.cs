using Logic.Pipelining;
using Xunit.Abstractions;

namespace Tests.Pipelining
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
            _output.WriteLine($"Logging request: {middlewareContext.Request}");
            
            await next(middlewareContext);
            
            _output.WriteLine($"Logging response: {middlewareContext.Response}");
        }

       
    }

}