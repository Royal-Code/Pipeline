
using RoyalCode.EventDispatcher;
using System;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

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