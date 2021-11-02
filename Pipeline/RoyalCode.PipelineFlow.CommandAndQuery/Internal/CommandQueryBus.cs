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
        
        public CommandQueryBus(IPipelineFactory<ICommandQueryBus> factory)
        {
            this.factory = factory;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send(IRequest request) => factory
            .Create(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))
            .Send(request);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult Send<TResult>(IRequest<TResult> request) => factory
            .Create<TResult>(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))
            .Send(request);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task SendAsync(IRequest request, CancellationToken token = default) => factory
            .Create(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))
            .SendAsync(request);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token = default) 
            => factory.Create<TResult>(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))
            .SendAsync(request);
    }
}
