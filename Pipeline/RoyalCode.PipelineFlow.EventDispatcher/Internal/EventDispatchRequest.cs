
using RoyalCode.EventDispatcher;
using System;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

#if NET5_0_OR_GREATER

internal record EventDispatchRequest<TEvent>(TEvent Event, DispatchStrategy Strategy) : IEventDispatchRequest<TEvent>
{
    public Type EventType => typeof(TEvent);

    public object EventObject => Event;
}

#else

internal class EventDispatchRequest<TEvent> : IEventDispatchRequest<TEvent>
{
    public EventDispatchRequest(TEvent @event, DispatchStrategy strategy)
    {
        Event = @event;
        Strategy = strategy;
    }

    public TEvent Event { get; }

    public DispatchStrategy Strategy { get; }

    public Type EventType => typeof(TEvent);

    public object EventObject => Event;
}

#endif