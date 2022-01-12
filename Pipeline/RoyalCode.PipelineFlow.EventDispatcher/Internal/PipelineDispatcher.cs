using RoyalCode.EventDispatcher;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

internal class PipelineDispatcher<TEvent> : IPipelineDispatcher
{
    private readonly IPipelineFactory<IEventDispatcher> factory;

    public PipelineDispatcher(IPipelineFactory<IEventDispatcher> factory)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public void Dispatch(object eventObject, DispatchStrategy strategy)
    {
        var pipeline = factory.Create<IEventDispatchRequest<TEvent>>();
        var request = new EventDispatchRequest<TEvent>((TEvent)eventObject, strategy);
        pipeline.Send(request);
    }

    public async Task DispatchAsync(object eventObject, DispatchStrategy strategy, CancellationToken cancellationToken)
    {
        var pipeline = factory.Create<IEventDispatchRequest<TEvent>>();
        var request = new EventDispatchRequest<TEvent>((TEvent)eventObject, strategy);
        await pipeline.SendAsync(request);
    }
}