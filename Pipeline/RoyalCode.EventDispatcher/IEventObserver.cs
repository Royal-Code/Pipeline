using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.EventDispatcher;

/// <summary>
/// <para>
///     A common interface to observe events.
/// </para>
/// <para>
///     To determine the stage in which the event will be observed, 
///     the <see cref="ObserveInCurrentScopeAttribute"/> 
///     and <see cref="ObserveInSeparetedScopeAttribute"/> can be used.
/// </para>
/// <para>
///     If none attribute was used, the default stage will be used.
/// </para>
/// </summary>
public interface IEventObserver<in TEvent>
{
    /// <summary>
    /// Receive the dispatched event.
    /// </summary>
    /// <param name="event">The event object, contains the event data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task of assynchronous operation.</returns>
    Task ReceiveAsync(TEvent @event, CancellationToken cancellationToken);
}
