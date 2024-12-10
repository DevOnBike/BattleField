using Logic.Pipelining;

namespace Tests
{
    public class LoggingMiddleware : IMiddleware
    {
        public async Task ExecuteAsync(MiddlewareContext middlewareContext, Task next)
        {
            Console.WriteLine($"Request: {middlewareContext.Request}");
            await next;
            Console.WriteLine($"Response: {middlewareContext.Response}");
        }

       
    }

}