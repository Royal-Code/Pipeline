using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.EventDispatcher;

/// <summary>
/// Extensions methods for <see cref="IEventDispatcher"/>.
/// </summary>
public static class EventDispatcherExtensions
{
    /// <summary>
    /// Dispatch one event to the observers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="dispatcher">The <see cref="IEventDispatcher"/>.</param>
    /// <param name="event">The object of the event, that contains de event data.</param>
    /// <param name="strategy">The strategy of the dispatch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task of assynchronous operation.</returns>
    public static Task DispatchAsync<TEvent>(this IEventDispatcher dispatcher,
        TEvent @event, DispatchStrategy strategy, CancellationToken cancellationToken = default)
        where TEvent : class
    {
        return dispatcher.DispatchAsync(typeof(TEvent), @event, strategy, cancellationToken);
    }

    /// <summary>
    /// Dispatch one event to the observers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="dispatcher">The <see cref="IEventDispatcher"/>.</param>
    /// <param name="event">The object of the event, that contains de event data.</param>
    /// <param name="strategy">The strategy of the dispatch.</param>
    public static void Dispatch<TEvent>(this IEventDispatcher dispatcher,
        TEvent @event, DispatchStrategy strategy)
        where TEvent : class
    {
        dispatcher.Dispatch(typeof(TEvent), @event, strategy);
    }

    /// <summary>
    /// Dispatch one event to the observers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="dispatcher">The <see cref="IEventDispatcher"/>.</param>
    /// <param name="event">The object of the event, that contains de event data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task of assynchronous operation.</returns>
    public static Task DispatchInCurrentScopeAsync<TEvent>(this IEventDispatcher dispatcher,
        TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class
    {
        return dispatcher.DispatchAsync(typeof(TEvent), @event, DispatchStrategy.InCurrentScope, cancellationToken);
    }

    /// <summary>
    /// Dispatch one event to the observers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="dispatcher">The <see cref="IEventDispatcher"/>.</param>
    /// <param name="event">The object of the event, that contains de event data.</param>
    public static void DispatchInCurrentScope<TEvent>(this IEventDispatcher dispatcher, TEvent @event)
        where TEvent : class
    {
        dispatcher.Dispatch(typeof(TEvent), @event, DispatchStrategy.InCurrentScope);
    }

    /// <summary>
    /// Dispatch one event to the observers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="dispatcher">The <see cref="IEventDispatcher"/>.</param>
    /// <param name="event">The object of the event, that contains de event data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task of assynchronous operation.</returns>
    public static Task DispatchInSeparetedScopeAsync<TEvent>(this IEventDispatcher dispatcher,
        TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class
    {
        return dispatcher.DispatchAsync(typeof(TEvent), @event, DispatchStrategy.InSeparetedScope, cancellationToken);
    }

    /// <summary>
    /// Dispatch one event to the observers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="dispatcher">The <see cref="IEventDispatcher"/>.</param>
    /// <param name="event">The object of the event, that contains de event data.</param>
    public static void DispatchInSeparetedScope<TEvent>(this IEventDispatcher dispatcher, TEvent @event)
        where TEvent : class
    {
        dispatcher.Dispatch(typeof(TEvent), @event, DispatchStrategy.InSeparetedScope);
    }

    /// <summary>
    /// Dispatch one event to the observers for all strategies (see <see cref="DispatchStrategy"/>).
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="dispatcher">The <see cref="IEventDispatcher"/>.</param>
    /// <param name="event">The object of the event, that contains de event data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task of assynchronous operation.</returns>
    public static async Task DispatchToAllStrategiesAsync<TEvent>(this IEventDispatcher dispatcher,
        TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class
    {
        await dispatcher.DispatchAsync(typeof(TEvent), @event, DispatchStrategy.InCurrentScope, cancellationToken);
        await dispatcher.DispatchAsync(typeof(TEvent), @event, DispatchStrategy.InSeparetedScope, cancellationToken);
    }

    /// <summary>
    /// Dispatch one event to the observers for all strategies (see <see cref="DispatchStrategy"/>).
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="dispatcher">The <see cref="IEventDispatcher"/>.</param>
    /// <param name="event">The object of the event, that contains de event data.</param>
    public static void DispatchToAllStrategies<TEvent>(this IEventDispatcher dispatcher, TEvent @event)
        where TEvent : class
    {
        dispatcher.Dispatch(typeof(TEvent), @event, DispatchStrategy.InCurrentScope);
        dispatcher.Dispatch(typeof(TEvent), @event, DispatchStrategy.InSeparetedScope);
    }
}