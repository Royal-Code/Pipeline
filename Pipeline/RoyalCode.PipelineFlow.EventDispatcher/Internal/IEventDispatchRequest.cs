using RoyalCode.EventDispatcher;
using System;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

public interface IEventDispatchRequest
{
    Type EventType { get; }

    object EventObject { get; }

    DispatchStrategy Strategy { get; }
}

public interface IEventDispatchRequest<TEvent> : IEventDispatchRequest
{
    TEvent Event { get; }
}