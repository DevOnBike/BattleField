namespace Logic.Pipelining
{
    public class InterfaceMiddlewareBinder
    {
        private readonly IMiddleware _middleware;

        public InterfaceMiddlewareBinder(IMiddleware middleware)
        {
            _middleware = middleware;
        }

        public MiddlewareDelegate CreateMiddleware(MiddlewareDelegate next)
        {
            return (context) => _middleware.ExecuteAsync(context, next);
        }
    }
}

