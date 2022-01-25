using System;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// Base class for the resquest to dispatch events.
/// </summary>
/// <typeparam name="TEvent">The event type.</typeparam>
internal class EventDispatchRequestBase<TEvent>
{
    /// <summary>
    /// The result.
    /// </summary>
    public EventDispatchResult Result { get; } = new EventDispatchResult();
}