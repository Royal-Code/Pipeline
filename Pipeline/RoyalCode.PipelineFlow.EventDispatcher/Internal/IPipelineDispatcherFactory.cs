using System;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

public interface IPipelineDispatcherFactory
{
    IPipelineDispatcher Create(Type eventType);
}



