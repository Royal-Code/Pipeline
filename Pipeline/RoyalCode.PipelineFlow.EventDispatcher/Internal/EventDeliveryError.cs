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
/// <param name="ObserverType">The type of event observer who generated the exception.</param>
/// <param name="Exception">An error message.</param>
/// <param name="Message">The exception throwed by the observer.</param>
internal record EventDeliveryError(Type ObserverType, Exception Exception, string Message)
{
    public override string ToString()
    {
        return $"Error: {Message}, observer type: {ObserverType.Name}, exception type: {Exception.GetType().Name}, message: {Exception.Message}";
    }
}