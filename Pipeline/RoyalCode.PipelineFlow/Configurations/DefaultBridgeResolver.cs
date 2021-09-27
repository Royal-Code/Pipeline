using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public static class DefaultBridgeResolver
    {
        public static IHandlerResolver BridgeHandler<TInput, TNextInput>(Action<TInput, Action<TNextInput>> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandlerAsync<TInput, TNextInput>(Func<TInput, Func<TNextInput, Task>, Task> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandlerAsync<TInput, TNextInput>(Func<TInput, Func<TNextInput, Task>, CancellationToken, Task> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TService, TInput, TNextInput>(Action<TService, TInput, Action<TNextInput>> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandlerAsync<TService, TInput, TNextInput>(Func<TService, TInput, Func<TNextInput, Task>, Task> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandlerAsync<TService, TInput, TNextInput>(Func<TService, TInput, Func<TNextInput, Task>, CancellationToken, Task> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));



        public static IHandlerResolver BridgeHandler<TInput, TOutput, TNextInput>(Func<TInput, Func<TNextInput, TOutput>, TOutput> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandlerAsync<TInput, TOutput, TNextInput>(Func<TInput, Func<TNextInput, Task<TOutput>>, Task<TOutput>> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandlerAsync<TInput, TOutput, TNextInput>(Func<TInput, Func<TNextInput, Task<TOutput>>, CancellationToken, Task<TOutput>> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TService, TInput, TOutput, TNextInput>(Func<TService, TInput, Func<TNextInput, TOutput>, TOutput> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandlerAsync<TService, TInput, TOutput, TNextInput>(Func<TService, TInput, Func<TNextInput, Task<TOutput>>, Task<TOutput>> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandlerAsync<TService, TInput, TOutput, TNextInput>(Func<TService, TInput, Func<TNextInput, Task<TOutput>>, CancellationToken, Task<TOutput>> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));



        public static IHandlerResolver BridgeHandler<TInput, TOutput, TNextInput, TNextOuput>(Func<TInput, Func<TNextInput, TNextOuput>, TOutput> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandlerAsync<TInput, TOutput, TNextInput, TNextOuput>(Func<TInput, Func<TNextInput, Task<TNextOuput>>, Task<TOutput>> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandlerAsync<TInput, TOutput, TNextInput, TNextOuput>(Func<TInput, Func<TNextInput, Task<TNextOuput>>, CancellationToken, Task<TOutput>> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TService, TInput, TOutput, TNextInput, TNextOuput>(Func<TService, TInput, Func<TNextInput, TNextOuput>, TOutput> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandlerAsync<TService, TInput, TOutput, TNextInput, TNextOuput>(Func<TService, TInput, Func<TNextInput, Task<TNextOuput>>, Task<TOutput>> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandlerAsync<TService, TInput, TOutput, TNextInput, TNextOuput>(Func<TService, TInput, Func<TNextInput, Task<TNextOuput>>, CancellationToken, Task<TOutput>> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));
    }
}
