using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.EventDispatcher;

/// <summary>
/// <para>
///     Handlers for when the event dispatch is successfully completed.
/// </para>
/// <para>
///     Events will only be handled in dispatches whose strategy is of separate scope.
/// </para>
/// <para>
///     Handle all types of events.
/// </para>
/// </summary>
public interface IEventDispatchCompletedHandler 
{
    /// <summary>
    /// Handler the event after dispatch is successfully completed.
    /// </summary>
    /// <param name="eventType">The event type.</param>
    /// <param name="eventObject">The event object, data.</param>
    /// <param name="observer">The observer that throw the exception.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task of assynchronous operation.</returns>
    Task HandleDispatchCompletedAsync(
        Type eventType, object eventObject,
        object observer, CancellationToken cancellationToken = default);
}

/// <summary>
/// <para>
///     Handlers for when the event dispatch is successfully completed.
/// </para>
/// <para>
///     Events will only be handled in dispatches whose strategy is of separate scope.
/// </para>
/// <para>
///     Handle events of type <typeparamref name="TEvent"/>.
/// </para>
/// </summary>
/// <typeparam name="TEvent">The event type to handle.</typeparam>
public interface IEventDispatchCompletedHandler<in TEvent>
{
    /// <summary>
    /// Handler the event after dispatch is successfully completed.
    /// </summary>
    /// <param name="eventObject">The event object, data.</param>
    /// <param name="observer">The observer that throw the exception.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task of assynchronous operation.</returns>
    Task HandleDispatchCompletedAsync(
        TEvent eventObject, object observer, CancellationToken cancellationToken = default);
}