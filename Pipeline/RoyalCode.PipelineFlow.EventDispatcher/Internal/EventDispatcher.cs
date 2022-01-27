using RoyalCode.EventDispatcher;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <inheritdoc/>
internal class EventDispatcher : IEventDispatcher
{
    
    private readonly IPipelineDispatcherFactory factory;

    /// <summary>
    /// <para>
    ///     Creates a new default implementation of <see cref="IEventDispatcher"/>.
    /// </para>
    /// </summary>
    /// <param name="factory">The factory to create a dispatcher pipeline.</param>
    /// <exception cref="ArgumentNullException">
    ///     If the factory is null.
    /// </exception>
    public EventDispatcher(IPipelineDispatcherFactory factory)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// <para>
    ///     Dispatch the event through the pipeline to the observers.
    /// </para>
    /// </summary>
    /// <param name="eventType">The event type.</param>
    /// <param name="eventObject">The event object.</param>
    /// <param name="strategy">The dispatch strategy.</param>
    public void Dispatch(Type eventType, object eventObject, DispatchStrategy strategy)
    {
        var pipeline = factory.Create(eventType);
        pipeline.Dispatch(eventObject, strategy);
    }

    /// <summary>
    /// <para>
    ///     Dispatch the event through the pipeline to the observers.
    /// </para>
    /// </summary>
    /// <param name="eventType">The event type.</param>
    /// <param name="eventObject">The event object.</param>
    /// <param name="strategy">The dispatch strategy.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task DispatchAsync(
        Type eventType, object eventObject, DispatchStrategy strategy,
        CancellationToken cancellationToken = default)
    {
        var pipeline = factory.Create(eventType);
        await pipeline.DispatchAsync(eventObject, strategy, cancellationToken);
    }
}
