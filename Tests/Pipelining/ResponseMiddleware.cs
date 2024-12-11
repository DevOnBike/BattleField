using Logic.Pipelining;
using Xunit.Abstractions;

namespace Tests.Pipelining
{
    public class ResponseMiddleware : IMiddleware
    {
        private readonly ITestOutputHelper _output;

        public ResponseMiddleware(ITestOutputHelper output)
        {
            _output = output;
        }

        public Task ExecuteAsync(MiddlewareContext middlewareContext, MiddlewareDelegate next)
        {
            _output.WriteLine("ResponseMiddleware ... before.");
            
            middlewareContext.Response = "Processed by ResponseMiddleware";
            
            return next(middlewareContext);
        }
        
    }

}