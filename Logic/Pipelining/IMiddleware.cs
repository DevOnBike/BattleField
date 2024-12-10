﻿namespace Logic.Pipelining
{
    public interface IMiddleware
    {
        Task ExecuteAsync(MiddlewareContext middlewareContext, Task next);
    }
}
