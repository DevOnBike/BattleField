namespace Logic.Pipelining
{
    // Define the middleware delegate
    public class MiddlewarePipeline
    {
        private readonly List<MiddlewareDelegate> _middlewares = [];

        public void Use(MiddlewareDelegate middleware)
        {
            _middlewares.Add(middleware);
        }

        public async Task ExecuteAsync(MiddlewareContext middlewareContext)
        {
            // Create the pipeline by chaining middleware together
            var next = () => Task.CompletedTask; // Default no-op

            for (var i = _middlewares.Count - 1; i >= 0; i--)
            {
                var currentMiddleware = _middlewares[i];
                var nextMiddleware = next;
                
                next = () => currentMiddleware(middlewareContext, nextMiddleware);
            }

            // Execute the pipeline
            await next();
        }
    }
}

