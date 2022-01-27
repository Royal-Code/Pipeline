using RoyalCode.EventDispatcher;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     Default implementation of <see cref="IPipelineDispatcherFactory"/>.
/// </para>
/// </summary>
internal class PipelineDispatcherFactory : IPipelineDispatcherFactory
{
    private static readonly ConcurrentDictionary<Type, Func<EventDispatcherPipelineFactory, IPipelineDispatcher>> 
        Creators = new();

    private readonly EventDispatcherPipelineFactory factory;

    /// <summary>
    /// <para>
    ///     Creates a new pipeline dispatcher factory.
    /// </para>
    /// </summary>
    /// <param name="factory">
    /// <para>
    ///     Internal service for create pipelines to dispatch events in current and in separated scope.
    /// </para>
    /// </param>
    public PipelineDispatcherFactory(EventDispatcherPipelineFactory factory)
    {
        this.factory = factory;
    }

    /// <summary>
    /// <para>
    ///     Creates the pipeline dispatcher for the event type.
    /// </para>
    /// </summary>
    /// <param name="eventType">The event type.</param>
    /// <returns>
    /// <para>
    ///     A new instance of <see cref="IPipelineDispatcher"/> to send events of type <paramref name="eventType"/>.
    /// </para>
    /// </returns>
    public IPipelineDispatcher Create(Type eventType)
    {
        var creator = Creators.GetOrAdd(eventType, GenerateCreator);
        return creator(factory);
    }

    /// <summary>
    /// Internal method to build a function that creates the pipeline dispatcher for the event type.
    /// </summary>
    /// <param name="eventType">The event type.</param>
    /// <returns>A factory function to create the pipeline dispatcher.</returns>
    private static Func<EventDispatcherPipelineFactory, IPipelineDispatcher> GenerateCreator(Type eventType)
    {
        var factoryParam = Expression.Parameter(typeof(EventDispatcherPipelineFactory), "factory");
        var pipelineDispatcherType = typeof(PipelineDispatcher<>).MakeGenericType(eventType);
        var ctor = pipelineDispatcherType.GetConstructors().First();

        var newPipelineDispatcher = Expression.New(ctor, factoryParam);

        var lambda = Expression.Lambda<Func<EventDispatcherPipelineFactory, IPipelineDispatcher>>(newPipelineDispatcher, factoryParam);

        return lambda.Compile();
    }
}
