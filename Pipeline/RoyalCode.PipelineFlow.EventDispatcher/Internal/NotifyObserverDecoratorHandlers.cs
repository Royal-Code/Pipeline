
using RoyalCode.EventDispatcher;
using System;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

internal static class NotifyObserverDecoratorHandlers
{
    public static async Task NotifyInCurrentScope<TObserver, TEvent>(
        TObserver observer,
        CurrentScopeEventDispatchRequest<TEvent> request,
        Func<Task> next)
        where TObserver : IEventObserver<TEvent>
        where TEvent : class
    {
        try
        {
            await observer.ReceiveAsync(request.Event);
            request.Result.Success();
        }
        catch (Exception ex)
        {
            request.Result.Failure(new EventDeliveryError(typeof(TObserver), ex, ""));
        }

        await next();
    }

    public static async Task NotifyInSeparatedScope<TObserver, TEvent>(
        TObserver observer,
        SeparatedScopeEventDispatchRequest<TEvent> request,
        Func<Task> next)
        where TObserver : IEventObserver<TEvent>
        where TEvent : class
    {
        try
        {
            await observer.ReceiveAsync(request.Event);
            request.Result.Success();
        }
        catch (Exception ex)
        {
            request.Result.Failure(new EventDeliveryError(typeof(TObserver), ex, ""));
        }

        await next();
    }
}

