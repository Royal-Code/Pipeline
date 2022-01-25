using RoyalCode.EventDispatcher;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     Dispatches the event through the pipeline (<see cref="IPipeline{TIn}"/>)
///     of the event barring <see cref="IEventDispatcher"/>.
/// </para>
/// </summary>
/// <typeparam name="TEvent">The event type.</typeparam>
internal class PipelineDispatcher<TEvent> : IPipelineDispatcher
{
    private readonly EventDispatcherPipelineFactory factory;
    private readonly DispatcherStateCollection stateCollection;

    /// <summary>
    /// Creates a new dispatcher.
    /// </summary>
    /// <param name="factory">The pipeline factory for event dispatchers.</param>
    public PipelineDispatcher(EventDispatcherPipelineFactory factory)
    {
        this.factory = factory;
        stateCollection = factory.DispatcherStateCollection;
    }

    /// <summary>
    /// Dispatch the event.
    /// </summary>
    /// <param name="eventObject">The event instance.</param>
    /// <param name="strategy">The dispatch strategy.</param>
    public void Dispatch(object eventObject, DispatchStrategy strategy)
    {
        if (strategy == DispatchStrategy.InCurrentScope)
        {
            var state = stateCollection.GetInCurrentScope(typeof(TEvent));
            if (!state.HasObservers)
                return;
            
            var pipeline = factory.CreatePipelineInCurrentScope<TEvent>();
            var request = new CurrentScopeEventDispatchRequest<TEvent>((TEvent) eventObject);
            pipeline.Send(request);

            if (request.Result.ThereIsNoObserverForTheEvent)
                state.HasObservers = false;
        }
        else
        {
            var state = stateCollection.GetInSeparetedScope(typeof(TEvent));
            if (!state.HasObservers)
                return;

            using var scope = factory.CreatePipelineInSeparatedScope<TEvent>();
            var pipeline = scope.Pipeline;
            var request = new SeparatedScopeEventDispatchRequest<TEvent>((TEvent)eventObject);
            pipeline.Send(request);
            
            if (request.Result.ThereIsNoObserverForTheEvent)
                state.HasObservers = false;
        }
    }

    /// <summary>
    /// Dispatch the event assynchronous.
    /// </summary>
    /// <param name="eventObject">The event instance.</param>
    /// <param name="strategy">The dispatch strategy.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task for async operation.</returns>
    public async Task DispatchAsync(object eventObject, DispatchStrategy strategy, CancellationToken cancellationToken)
    {
        if (strategy == DispatchStrategy.InCurrentScope)
        {
            var state = stateCollection.GetInCurrentScope(typeof(TEvent));
            if (!state.HasObservers)
                return;
            
            var pipeline = factory.CreatePipelineInCurrentScope<TEvent>();
            var request = new CurrentScopeEventDispatchRequest<TEvent>((TEvent) eventObject);
            await pipeline.SendAsync(request, cancellationToken);
            
            if (request.Result.ThereIsNoObserverForTheEvent)
                state.HasObservers = false;
        }
        else
        {
            var state = stateCollection.GetInSeparetedScope(typeof(TEvent));
            if (!state.HasObservers)
                return;
            
            using var scope = factory.CreatePipelineInSeparatedScope<TEvent>();
            var pipeline = scope.Pipeline;
            var request = new SeparatedScopeEventDispatchRequest<TEvent>((TEvent) eventObject);
            await pipeline.SendAsync(request, cancellationToken);
            
            if (request.Result.ThereIsNoObserverForTheEvent)
                state.HasObservers = false;
        }
    }
}