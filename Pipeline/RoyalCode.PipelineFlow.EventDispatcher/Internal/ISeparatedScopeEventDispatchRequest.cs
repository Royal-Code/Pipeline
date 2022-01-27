namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     Interface of the request to send a event to a pipeline dispatcher.
/// </para>
/// <para>
///     This is specific for separated scope dispatch strategy.
/// </para>
/// </summary>
/// <typeparam name="TEvent">The event type.</typeparam>
internal interface ISeparatedScopeEventDispatchRequest<TEvent> : IEventDispatchRequest<TEvent> { }