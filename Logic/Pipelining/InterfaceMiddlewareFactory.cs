namespace Logic.Pipelining
{
    public class InterfaceMiddlewareFactory
    {
        private readonly IMiddleware _middleware;

        public InterfaceMiddlewareFactory(IMiddleware middleware)
        {
            _middleware = middleware;
        }

        public MiddlewareDelegate CreateMiddleware(MiddlewareDelegate next)
        {
            return (context) => _middleware.ExecuteAsync(context, next);
        }
    }
}

