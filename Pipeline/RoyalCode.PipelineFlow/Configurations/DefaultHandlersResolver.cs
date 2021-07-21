using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public static class DefaultHandlersResolver
    {
        #region Handlers


        public static IHandlerResolver Handle<TInput>(Action<TInput> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TInput>(Func<TInput, Task> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TInput>(Func<TInput, CancellationToken, Task> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TService, TInput>(Action<TService, TInput> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver Handle<TService, TInput>(Func<TService, TInput, Task> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver Handle<TService, TInput>(Func<TService, TInput, CancellationToken, Task> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));



        public static IHandlerResolver Handle<TInput, TOutput>(Func<TInput, TOutput> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TInput, TOutput>(Func<TInput, Task<TOutput>> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TInput, TOutput>(Func<TInput, CancellationToken, Task<TOutput>> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TService, TInput, TOutput>(Func<TService, TInput, TOutput> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver Handle<TService, TInput, TOutput>(Func<TService, TInput, Task<TOutput>> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver Handle<TService, TInput, TOutput>(Func<TService, TInput, CancellationToken, Task<TOutput>> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));


        #endregion


        #region Bridges


        public static IHandlerResolver BridgeHandler<TInput, TNextInput>(Action<TInput, Action<TNextInput>> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TInput, TNextInput>(Func<TInput, Func<TNextInput, Task>, Task> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TInput, TNextInput>(Func<TInput, Func<TNextInput, CancellationToken, Task>, Task> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TService, TInput, TNextInput>(Action<TService, TInput, Action<TNextInput>> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandler<TService, TInput, TNextInput>(Func<TService, TInput, Func<TNextInput, Task>, Task> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandler<TService, TInput, TNextInput>(Func<TService, TInput, Func<TNextInput, Task>, CancellationToken, Task> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));



        public static IHandlerResolver BridgeHandler<TInput, TOutput, TNextInput>(Func<TInput, Func<TNextInput, TOutput>, TOutput> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TInput, TOutput, TNextInput>(Func<TInput, Func<TNextInput, Task<TOutput>>, Task<TOutput>> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TInput, TOutput, TNextInput>(Func<TInput, Func<TNextInput, Task<TOutput>>, CancellationToken, Task<TOutput>> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TService, TInput, TOutput, TNextInput>(Func<TService, TInput, Func<TNextInput, TOutput>, TOutput> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandler<TService, TInput, TOutput, TNextInput>(Func<TService, TInput, Func<TNextInput, Task<TOutput>>, Task<TOutput>> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandler<TService, TInput, TOutput, TNextInput>(Func<TService, TInput, Func<TNextInput, Task<TOutput>>, CancellationToken, Task<TOutput>> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));



        public static IHandlerResolver BridgeHandler<TInput, TOutput, TNextInput, TNextOuput>(Func<TInput, Func<TNextInput, TNextOuput>, TOutput> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TInput, TOutput, TNextInput, TNextOuput>(Func<TInput, Func<TNextInput, Task<TNextOuput>>, Task<TOutput>> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TInput, TOutput, TNextInput, TNextOuput>(Func<TInput, Func<TNextInput, Task<TNextOuput>>, CancellationToken, Task<TOutput>> handler)
            => new DelegateBridgeHandlerResolver(handler);

        public static IHandlerResolver BridgeHandler<TService, TInput, TOutput, TNextInput, TNextOuput>(Func<TService, TInput, Func<TNextInput, TNextOuput>, TOutput> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandler<TService, TInput, TOutput, TNextInput, TNextOuput>(Func<TService, TInput, Func<TNextInput, Task<TNextOuput>>, Task<TOutput>> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver BridgeHandler<TService, TInput, TOutput, TNextInput, TNextOuput>(Func<TService, TInput, Func<TNextInput, Task<TNextOuput>>, CancellationToken, Task<TOutput>> handler)
            => new ServiceAndDelegateBridgeHandlerResolver(handler, typeof(TService));


        #endregion
    }
}
