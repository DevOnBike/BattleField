namespace Logic.Pipelining
{
    public class MiddlewarePipeline
    {
        private readonly List<MiddlewareDelegate> _middlewares = [];

        public void Use(MiddlewareDelegate middleware)
        {
            _middlewares.Add(middleware);
        }

        public Task ExecuteAsync(MiddlewareContext middlewareContext)
        {
            var next = () => Task.CompletedTask; // Default no-op

            for (var i = _middlewares.Count - 1; i >= 0; i--)
            {
                var currentMiddleware = _middlewares[i];
                var nextMiddleware = next;
                
                next = () => currentMiddleware(middlewareContext, nextMiddleware);
            }

            return next();
        }
    }
}

