using System;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     This is an internal component for storing errors that occur during the delivery of events to observers.
/// </para>
/// <para>
///     These errors are exceptions that occur within the observers.
/// </para>
/// </summary>
internal class EventDeliveryError
{
    /// <summary>
    /// Creates a new error.
    /// </summary>
    /// <param name="observerType">The type of event observer who generated the exception.</param>
    /// <param name="exception">An error message.</param>
    /// <param name="message">The exception throwed by the observer.</param>
    public EventDeliveryError(Type observerType, Exception exception, string message)
    {
        ObserverType = observerType;
        Exception = exception;
        Message = message;
    }

    /// <summary>
    /// The type of event observer who generated the exception.
    /// </summary>
    public Type ObserverType { get; }
    
    /// <summary>
    /// An error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// The exception throwed by the observer.
    /// </summary>
    public Exception Exception { get; }
}