using System;
using RoyalCode.EventDispatcher;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

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
