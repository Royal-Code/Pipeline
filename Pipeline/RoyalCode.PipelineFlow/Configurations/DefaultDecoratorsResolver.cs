﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public static class DefaultDecoratorsResolver
    {
        public static IDecoratorResolver Decorate<TInput>(Action<TInput, Action> handler)
            => new DelegateDecoratorResolver(handler);

        public static IDecoratorResolver Decorate<TInput>(Func<TInput, Func<Task>, Task> handler)
            => new DelegateDecoratorResolver(handler);

        public static IDecoratorResolver Decorate<TInput>(Func<TInput, Func<Task>, CancellationToken, Task> handler)
            => new DelegateDecoratorResolver(handler);

        public static IDecoratorResolver Decorate<TService, TInput>(Action<TService, TInput, Action> handler)
            => new ServiceAndDelegateDecoratorResolver(handler, typeof(TService));

        public static IDecoratorResolver Decorate<TService, TInput>(Func<TService, TInput, Func<Task>, Task> handler)
            => new ServiceAndDelegateDecoratorResolver(handler, typeof(TService));

        public static IDecoratorResolver Decorate<TService, TInput>(Func<TService, TInput, Func<Task>, CancellationToken, Task> handler)
            => new ServiceAndDelegateDecoratorResolver(handler, typeof(TService));



        public static IDecoratorResolver Decorate<TInput, TOutput>(Func<TInput, Func<TOutput>, TOutput> handler)
            => new DelegateDecoratorResolver(handler);

        public static IDecoratorResolver Decorate<TInput, TOutput>(Func<TInput, Func<Task<TOutput>>, Task<TOutput>> handler)
            => new DelegateDecoratorResolver(handler);

        public static IDecoratorResolver Decorate<TInput, TOutput>(Func<TInput, Func<Task<TOutput>>, CancellationToken, Task<TOutput>> handler)
            => new DelegateDecoratorResolver(handler);

        public static IDecoratorResolver Decorate<TService, TInput, TOutput>(Func<TService, TInput, Func<TOutput>, TOutput> handler)
            => new ServiceAndDelegateDecoratorResolver(handler, typeof(TService));

        public static IDecoratorResolver Decorate<TService, TInput, TOutput>(Func<TService, TInput, Func<Task<TOutput>>, Task<TOutput>> handler)
            => new ServiceAndDelegateDecoratorResolver(handler, typeof(TService));

        public static IDecoratorResolver Decorate<TService, TInput, TOutput>(Func<TService, TInput, Func<Task<TOutput>>, CancellationToken, Task<TOutput>> handler)
            => new ServiceAndDelegateDecoratorResolver(handler, typeof(TService));
    }
}