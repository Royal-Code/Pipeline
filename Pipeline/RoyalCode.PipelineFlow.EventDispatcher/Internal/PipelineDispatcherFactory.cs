using RoyalCode.EventDispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

internal class PipelineDispatcherFactory : IPipelineDispatcherFactory
{
    private static readonly Dictionary<Type, Func<EventDispatcherPipelineFactory, IPipelineDispatcher>> creators = new();

    private readonly EventDispatcherPipelineFactory factory;

    public PipelineDispatcherFactory(EventDispatcherPipelineFactory factory)
    {
        this.factory = factory;
    }

    public IPipelineDispatcher Create(Type eventType)
    {
        if (!creators.TryGetValue(eventType, out var creator))
        {
            creator = GenerateCreator(eventType);
            creators.Add(eventType, creator);
        }

        return creator(factory);
    }

    private Func<EventDispatcherPipelineFactory, IPipelineDispatcher> GenerateCreator(Type eventType)
    {
        var factoryParam = Expression.Parameter(typeof(EventDispatcherPipelineFactory), "factory");
        var pipelineDispatcherType = typeof(PipelineDispatcher<>).MakeGenericType(eventType);
        var ctor = pipelineDispatcherType.GetConstructors().First();

        var newPipelineDispatcher = Expression.New(ctor, factoryParam);

        var lambda = Expression.Lambda<Func<EventDispatcherPipelineFactory, IPipelineDispatcher>>(newPipelineDispatcher, factoryParam);

        return lambda.Compile();
    }
}



