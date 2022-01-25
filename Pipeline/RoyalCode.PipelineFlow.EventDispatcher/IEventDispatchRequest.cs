using System;
using RoyalCode.EventDispatcher;

namespace RoyalCode.PipelineFlow.EventDispatcher;

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