using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Resolvers
{
    /// <summary>
    /// Contains method factories for create all kinds of <see cref="IHandlerResolver"/> for processing handlers.
    /// </summary>
    public static class DefaultHandlersResolver
    {
        public static IHandlerResolver Handle<TInput>(Action<TInput> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver HandleAsync<TInput>(Func<TInput, Task> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver HandleAsync<TInput>(Func<TInput, CancellationToken, Task> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TService, TInput>(Action<TService, TInput> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver HandleAsync<TService, TInput>(Func<TService, TInput, Task> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver HandleAsync<TService, TInput>(Func<TService, TInput, CancellationToken, Task> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));



        public static IHandlerResolver Handle<TInput, TOutput>(Func<TInput, TOutput> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver HandleAsync<TInput, TOutput>(Func<TInput, Task<TOutput>> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver HandleAsync<TInput, TOutput>(Func<TInput, CancellationToken, Task<TOutput>> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TService, TInput, TOutput>(Func<TService, TInput, TOutput> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver HandleAsync<TService, TInput, TOutput>(Func<TService, TInput, Task<TOutput>> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver HandleAsync<TService, TInput, TOutput>(Func<TService, TInput, CancellationToken, Task<TOutput>> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));
    }
}
