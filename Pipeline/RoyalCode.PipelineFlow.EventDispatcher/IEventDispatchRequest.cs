using System;
using RoyalCode.EventDispatcher;

namespace RoyalCode.PipelineFlow.EventDispatcher;

/// <summary>
/// <para>
///     Base interface of the request to dispatch an event.
/// </para>
/// <para>
///     When an event is dispatched through <see cref="IEventDispatcher"/>, a request is created and sent
///     to a pipeline event dispatcher. This interfaces is the base type for the event request.
/// </para>
/// </summary>
public interface IEventDispatchRequest
{
    /// <summary>
    /// The event type.
    /// </summary>
    Type EventType { get; }

    /// <summary>
    /// The event object.
    /// </summary>
    object EventObject { get; }

    /// <summary>
    /// The dispatch strategy.
    /// </summary>
    DispatchStrategy Strategy { get; }
}

/// <summary>
/// <para>
///     Base interface of the request to dispatch an event.
/// </para>
/// <para>
///     This interface is an extension of <see cref="IEventDispatchRequest"/> with the event type as a generic
///     parameter.
/// </para>
/// </summary>
/// <typeparam name="TEvent">The event type.</typeparam>
public interface IEventDispatchRequest<TEvent> : IEventDispatchRequest
{
    /// <summary>
    /// The event object.
    /// </summary>
    TEvent Event { get; }
}