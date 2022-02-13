using System;
using Microsoft.Extensions.DependencyInjection;
using RoyalCode.EventDispatcher;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     Internal service for create pipelines to dispatch events in current and in separated scope.
/// </para>
/// </summary>
internal class EventDispatcherPipelineFactory
{
    private readonly IServiceProvider serviceProvider;
    private readonly IPipelineFactory<IEventDispatcher> factory;

    /// <summary>
    /// Creates a new instance of this service.
    /// </summary>
    /// <param name="serviceProvider">The service provider, to create service scopes.</param>
    /// <param name="factory">The pipeline factory of the current scope.</param>
    /// <param name="dispatcherStateCollection">
    ///     A collection containing the state of the dispatchers by event type and dispatch strategy.
    /// </param>
    public EventDispatcherPipelineFactory(
        IServiceProvider serviceProvider,
        IPipelineFactory<IEventDispatcher> factory,
        DispatcherStateCollection dispatcherStateCollection)
    {
        this.serviceProvider = serviceProvider;
        this.factory = factory;
        DispatcherStateCollection = dispatcherStateCollection;
    }
    
    /// <summary>
    /// A collection containing the state of the dispatchers by event type and dispatch strategy.
    /// </summary>
    public DispatcherStateCollection DispatcherStateCollection { get; }

    /// <summary>
    /// Creates a new pipeline to dispatch events in current scope.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <returns>New <see cref="IPipeline{TIn}"/>.</returns>
    public IPipeline<CurrentScopeEventDispatchRequest<TEvent>> CreatePipelineInCurrentScope<TEvent>()
        where TEvent : class
        => factory.Create<CurrentScopeEventDispatchRequest<TEvent>>();
    
    /// <summary>
    /// Creates a scoped pipeline that wraps the pipeline for dispatch events in separated scope.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <returns>A new instance of <see cref="ScopedPipeline{TIn}"/>.</returns>
    public ScopedPipeline<SeparatedScopeEventDispatchRequest<TEvent>> CreatePipelineInSeparatedScope<TEvent>()
        where TEvent: class
        => new(serviceProvider.CreateScope());
}