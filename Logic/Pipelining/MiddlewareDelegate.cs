namespace Logic.Pipelining
{
    public delegate Task MiddlewareDelegate(MiddlewareContext middlewareContext, Func<Task> next);
}

