using RoyalCode.EventDispatcher;
using System;
using System.Collections.Generic;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

internal class PipelineDispatcherFactory : IPipelineDispatcherFactory
{
    private static readonly Dictionary<Type, Func<IPipelineFactory<IEventDispatcher>, IPipelineDispatcher>> creators = new();

    private readonly IPipelineFactory<IEventDispatcher> factory;

    public PipelineDispatcherFactory(IPipelineFactory<IEventDispatcher> factory)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
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

    private Func<IPipelineFactory<IEventDispatcher>, IPipelineDispatcher> GenerateCreator(Type eventType)
    {
        // User expressions para criar função lambda.
        // compilar e retornar.

        throw new NotImplementedException();
    }
}



