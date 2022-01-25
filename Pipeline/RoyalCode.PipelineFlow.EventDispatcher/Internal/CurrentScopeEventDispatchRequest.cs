
using RoyalCode.EventDispatcher;
using System;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

#if NET5_0_OR_GREATER

internal record CurrentScopeEventDispatchRequest<TEvent>(TEvent Event) : ICurrentScopeEventDispatchRequest<TEvent>
{
    public Type EventType => typeof(TEvent);

    public object EventObject => Event;

    public DispatchStrategy Strategy => DispatchStrategy.InCurrentScope;
    
    /// <summary>
    /// The dispatch result.
    /// </summary>
    public EventDispatchResult Result { get; } = new EventDispatchResult();
}

#else

internal class CurrentScopeEventDispatchRequest<TEvent> : ICurrentScopeEventDispatchRequest<TEvent>
{
    public CurrentScopeEventDispatchRequest(TEvent @event)
    {
        Event = @event;
    }

    public TEvent Event { get; }

    public DispatchStrategy Strategy => DispatchStrategy.InCurrentScope;

    public Type EventType => typeof(TEvent);

    public object EventObject => Event;

    /// <summary>
    /// The dispatch result.
    /// </summary>
    public EventDispatchResult Result { get; } = new EventDispatchResult();
}

#endif