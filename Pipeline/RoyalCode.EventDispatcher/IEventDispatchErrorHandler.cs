using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.EventDispatcher;

/// <summary>
/// <para>
///     Error handlers when dispatching events to observers.
/// </para>
/// <para>
///     Errors will only be handled in dispatches whose strategy is of separate scope.
/// </para>
/// <para>
///     Handle all types of events.
/// </para>
/// </summary>
public interface IEventDispatchErrorHandler
{
    /// <summary>
    /// Handler the event error.
    /// </summary>
    /// <param name="eventType">The event type.</param>
    /// <param name="eventObject">The event object, data.</param>
    /// <param name="observer">The observer that throw the exception.</param>
    /// <param name="exception">The exception, error.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task of assynchronous operation.</returns>
    Task HandleErrorAsync(
        Type eventType, object eventObject,
        object observer, Exception exception,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// <para>
///     Error handlers when dispatching events to observers.
/// </para>
/// <para>
///     Errors will only be handled in dispatches whose strategy is of separate scope.
/// </para>
/// <para>
///     Handle events of type <typeparamref name="TEvent"/>.
/// </para>
/// </summary>
/// <typeparam name="TEvent">The event type to handle.</typeparam>
public interface IEventDispatchErrorHandler<TEvent>
    where TEvent : class
{
    /// <summary>
    /// Handler the event error.
    /// </summary>
    /// <param name="eventObject">The event object, data.</param>
    /// <param name="observer">The observer that throw the exception.</param>
    /// <param name="exception">The exception, error.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task of assynchronous operation.</returns>
    Task HandleErrorAsync(
        TEvent eventObject,
        object observer, Exception exception,
        CancellationToken cancellationToken = default);
}
