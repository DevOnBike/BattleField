using Logic.Pipelining;

namespace Tests
{
    public class ResponseMiddleware : IMiddleware
    {
        public async Task ExecuteAsync(MiddlewareContext middlewareContext, Task next)
        {
            middlewareContext.Response = "Processed by ResponseMiddleware";
            await next;
        }
        
    }

}