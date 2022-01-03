
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.EventDispatcher;

/// <summary>
/// <para>
///     The event dispatcher is a mediator between those who create and dispatch events and the observers, 
///     event handlers.
/// </para>
/// <para>
///     The events can be dispatched in two fases, the first in the same transaction or unit of work,
///     and the second in a seperated scope, after of the unit of work or transaction.
/// </para>
/// <para>
///     These two ways of dispatching events will cause events to be delivered to observers
///     who are interested in receiving the events at each stage.
/// </para>
/// <para>
///     Who dispatches the events determines which stage the events are being dispatched.
/// </para>
/// <para>
///     The observers decide when they want to listen to the events, in which stage they want to listen to the events.
/// </para>
/// </summary>
public interface IEventDispatcher
{
    /// <summary>
    /// Dispatch one event to the observers.
    /// </summary>
    /// <param name="eventType">The type of the event.</param>
    /// <param name="eventObject">The object of the event, that contains de event data.</param>
    /// <param name="strategy">The strategy of the dispatch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task of assynchronous operation.</returns>
    Task DispatchAsync(Type eventType, object eventObject, DispatchStrategy strategy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dispatch one event to the observers.
    /// </summary>
    /// <param name="eventType">The type of the event.</param>
    /// <param name="eventObject">The object of the event, that contains de event data.</param>
    /// <param name="strategy">The strategy of the dispatch.</param>
    void Dispatch(Type eventType, object eventObject, DispatchStrategy strategy);
}
