using RoyalCode.EventDispatcher;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     Component that will send the event dispatched by <see cref="IEventDispatcher"/> to the event pipeline.
/// </para>
/// </summary>
public interface IPipelineDispatcher
{
    /// <summary>
    /// Dispatch the evento to the pipeline.
    /// </summary>
    /// <param name="eventObject">The event object.</param>
    /// <param name="strategy">The dispatch strategy.</param>
    void Dispatch(object eventObject, DispatchStrategy strategy);

    /// <summary>
    /// Dispatch the evento to the pipeline.
    /// </summary>
    /// <param name="eventObject">The event object.</param>
    /// <param name="strategy">The dispatch strategy.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task for async operation.</returns>
    Task DispatchAsync(object eventObject, DispatchStrategy strategy, CancellationToken cancellationToken);
}


