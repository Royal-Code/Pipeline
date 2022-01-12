using RoyalCode.EventDispatcher;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

internal class EventDispatcher : IEventDispatcher
{
    
    private readonly IPipelineDispatcherFactory factory;

    public EventDispatcher(IPipelineDispatcherFactory factory)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public void Dispatch(Type eventType, object eventObject, DispatchStrategy strategy)
    {
        var pipeline = factory.Create(eventType);
        pipeline.Dispatch(eventObject, strategy);
    }

    public async Task DispatchAsync(
        Type eventType, object eventObject, DispatchStrategy strategy,
        CancellationToken cancellationToken = default)
    {
        var pipeline = factory.Create(eventType);
        await pipeline.DispatchAsync(eventObject, strategy, cancellationToken);
    }
}
