using RoyalCode.EventDispatcher;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

internal class EventDispatcher : IEventDispatcher
{
    private readonly IPipelineFactory<IEventDispatcher> factory;

    public void Dispatch(Type eventType, object eventObject, DispatchStrategy strategy)
    {
        var request = new EventDispatchRequest();
        var pipeline = factory.Create<EventDispatchRequest>();
        pipeline.Send(request);
    }

    public Task DispatchAsync(Type eventType, object eventObject, DispatchStrategy strategy, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

#if NET5_0_OR_GREATER
public record EventDispatchRequest(){}

#else
public class EventDispatchRequest
{

}
#endif