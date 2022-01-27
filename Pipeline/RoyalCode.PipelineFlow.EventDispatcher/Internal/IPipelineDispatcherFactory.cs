using System;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     Component to create <see cref="IPipelineDispatcher"/> for each event type.
/// </para>
/// </summary>
public interface IPipelineDispatcherFactory
{
    /// <summary>
    /// <para>
    ///     Creates a <see cref="IPipelineDispatcher"/> for the event type.
    /// </para>
    /// </summary>
    /// <param name="eventType">The event type to be sended via pipeline.</param>
    /// <returns>An instance of <see cref="IPipelineDispatcher"/> to send events of type <paramref name="eventType"/>.</returns>
    IPipelineDispatcher Create(Type eventType);
}



