using System;
using RoyalCode.EventDispatcher;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

#if NET5_0_OR_GREATER

internal record SeparatedScopeEventDispatchRequest<TEvent>(TEvent Event) : ISeparatedScopeEventDispatchRequest<TEvent>
{
    public Type EventType => typeof(TEvent);

    public object EventObject => Event;

    public DispatchStrategy Strategy => DispatchStrategy.InSeparetedScope;
    
    /// <summary>
    /// The dispatch result.
    /// </summary>
    public EventDispatchResult Result { get; } = new EventDispatchResult();
}

#else

internal class SeparatedScopeEventDispatchRequest<TEvent> : ISeparatedScopeEventDispatchRequest<TEvent>
{
    public SeparatedScopeEventDispatchRequest(TEvent @event)
    {
        Event = @event;
    }

    public TEvent Event { get; }

    public DispatchStrategy Strategy => DispatchStrategy.InSeparetedScope;

    public Type EventType => typeof(TEvent);

    public object EventObject => Event;

    /// <summary>
    /// The dispatch result.
    /// </summary>
    public EventDispatchResult Result { get; } = new EventDispatchResult();
}

#endif