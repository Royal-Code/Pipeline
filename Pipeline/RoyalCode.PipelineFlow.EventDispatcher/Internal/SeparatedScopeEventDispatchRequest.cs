using System;
using RoyalCode.EventDispatcher;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     Default implementation of <see cref="ISeparatedScopeEventDispatchRequest{TEvent}"/>.
/// </para>
/// <para>
///     This class is used by the <see cref="PipelineDispatcher{TEvent}"/> to send the event through the pipeline
///     to the observers.
/// </para>
/// </summary>
/// <param name="Event">The event object.</param>
/// <typeparam name="TEvent">The type of the event.</typeparam>
internal record SeparatedScopeEventDispatchRequest<TEvent>(TEvent Event) : ISeparatedScopeEventDispatchRequest<TEvent>
    where TEvent : class
{
    /// <inheritdoc />
    public Type EventType => typeof(TEvent);

    /// <inheritdoc />
    public object EventObject => Event;

    /// <inheritdoc />
    public DispatchStrategy Strategy => DispatchStrategy.InSeparetedScope;
    
    /// <summary>
    /// The dispatch result.
    /// </summary>
    public EventDispatchResult Result { get; } = new EventDispatchResult();
}
