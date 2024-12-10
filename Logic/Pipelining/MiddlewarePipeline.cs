﻿namespace Logic.Pipelining
{
    public class MiddlewarePipeline
    {
        private readonly List<Func<MiddlewareDelegate, MiddlewareDelegate>> _middlewares = [];

        public void Use(Func<MiddlewareDelegate, MiddlewareDelegate> middleware)
        {
            _middlewares.Add(middleware);
        }

        public MiddlewareDelegate Build()
        {
            MiddlewareDelegate final = (ctx) => Task.CompletedTask;

            for (var i = _middlewares.Count - 1; i >= 0; i--)
            {
                var next = final;
                final = _middlewares[i](next);
            }

            return final;
        }

        public Task ExecuteAsync(MiddlewareContext middlewareContext)
        {
            var pipeline = Build();

            return pipeline(middlewareContext);
        }
    }
}

