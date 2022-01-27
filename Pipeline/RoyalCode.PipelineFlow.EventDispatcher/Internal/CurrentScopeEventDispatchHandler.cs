using Microsoft.Extensions.Logging;
using RoyalCode.EventDispatcher;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

internal class CurrentScopeEventDispatchHandler<TEvent>
    where TEvent : class
{
    private readonly ILogger logger;

    public CurrentScopeEventDispatchHandler(ILogger<IEventDispatcher> logger)
    {
        this.logger = logger;
    }
    
    public void CurrentScopeEventDispatch(CurrentScopeEventDispatchRequest<TEvent> request)
    {
        var logMessage = request.Result.ThereIsNoObserverForTheEvent
            ? "The event dispatch ended, no observers were found for the event dispatched with in current scope strategy. In the next dispatch there will be no attempt of delivery."
            : request.Result.HasErrors
                ? $"The event was dispatched to observers in current scope, but errors occurred. Total number of observers: {request.Result.DeliveryCount}. Errors found: {string.Join("\n", request.Result.Errors)}"
                : $"The event was successfully dispatched to observers in current scope. Total number of observers: {request.Result.DeliveryCount}.";
        
        logger.LogDebug(logMessage);
    }
}