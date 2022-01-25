using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     This is an internal component that stores the result of the event dispatch for each of the observers.
/// </para>
/// </summary>
internal class EventDispatchResult
{
    private ICollection<EventDeliveryError>? _errors;
    
    /// <summary>
    /// It tells how many observers the event was delivered to.
    /// </summary>
    public int DeliveryCount { get; private set; }

    /// <summary>
    /// Errors occurring in the delivery of the event to the observer, which are exceptions occurring in the observer.
    /// </summary>
    public IEnumerable<EventDeliveryError> Errors => _errors ?? Enumerable.Empty<EventDeliveryError>();

    /// <summary>
    /// Marks that the event has been successfully handed over to an observer.
    /// </summary>
    public void Success() => DeliveryCount++;

    /// <summary>
    /// Marks that the event was not successfully delivered, an error occurred in the observer.
    /// The error will be added to the result.
    /// </summary>
    /// <param name="error">The error occurred when delivering the message to the observer.</param>
    public void Failure(EventDeliveryError error)
    {
        DeliveryCount++;
        _errors ??= new LinkedList<EventDeliveryError>();
        _errors.Add(error);
    }

    /// <summary>
    /// There is no observer for the event.
    /// </summary>
    public bool ThereIsNoObserverForTheEvent => DeliveryCount is 0;
}