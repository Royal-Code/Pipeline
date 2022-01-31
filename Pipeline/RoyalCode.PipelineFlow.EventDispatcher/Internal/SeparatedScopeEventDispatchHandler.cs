using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RoyalCode.EventDispatcher;
using RoyalCode.PipelineFlow.EventDispatcher.Options;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// Internal handler for the <see cref="SeparatedScopeEventDispatchRequest{TEvent}"/>.
/// </summary>
/// <typeparam name="TEvent">The event dispatched type.</typeparam>
internal class SeparatedScopeEventDispatchHandler<TEvent>
    where TEvent : class
{
    private readonly ILogger logger;
    private readonly IOptions<EventDispatcherOptions> options;

    public SeparatedScopeEventDispatchHandler(
        ILogger<IEventDispatcher> logger,
        IOptions<EventDispatcherOptions> options)
    {
        this.logger = logger;
        this.options = options;
    }

    /// <summary>
    /// <para>
    ///     The handler will log the result of the dispatched.
    /// </para>
    /// <para>
    ///     The observers will be executed by decorators.
    /// </para>
    /// </summary>
    /// <param name="request">The event dispatch request.</param>
    public void CurrentScopeEventDispatch(SeparatedScopeEventDispatchRequest<TEvent> request)
    {
        var level = options.Value.LogLevel;
        if (!logger.IsEnabled(level))
            return;

        var logMessage = request.Result.ThereIsNoObserverForTheEvent
            ? "The event dispatch ended, no observers were found for the event dispatched with in separeted scope strategy. In the next dispatch there will be no attempt of delivery."
            : request.Result.HasErrors
                ? $"The event was dispatched to observers in separeted scope, but errors occurred. Total number of observers: {request.Result.DeliveryCount}. Errors found: {string.Join("\n", request.Result.Errors)}"
                : $"The event was successfully dispatched to observers in separeted scope. Total number of observers: {request.Result.DeliveryCount}.";

        logger.Log(level, logMessage);
    }
}

