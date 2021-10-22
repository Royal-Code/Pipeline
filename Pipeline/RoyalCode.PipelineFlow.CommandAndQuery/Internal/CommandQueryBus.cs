using RoyalCode.CommandAndQuery;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.CommandAndQuery.Internal
{
    internal class CommandQueryBus : ICommandQueryBus
    {
        private readonly IPipelineFactory<ICommandQueryBus> factory;
        private readonly RequestDispatchers dispatchers;

        public CommandQueryBus(IPipelineFactory<ICommandQueryBus> factory, RequestDispatchers dispatchers)
        {
            this.factory = factory;
            this.dispatchers = dispatchers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send(IRequest request)
            => dispatchers.GetDispatcher(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))(request, factory);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult Send<TResult>(IRequest<TResult> request)
            => dispatchers.GetDispatcher<TResult>(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))(request, factory);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task SendAsync(IRequest request, CancellationToken token = default)
            => dispatchers.GetAsyncDispatcher(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))(request, factory);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token = default)
            => dispatchers.GetAsyncDispatcher<TResult>(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))(request, factory);
    }
}
