
using RoyalCode.EventDispatcher;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

internal static class NotifyObserverDecoratorHandlers
{
    public static async Task NotifyInCurrentScope<TObserver, TEvent>(
        TObserver observer,
        CurrentScopeEventDispatchRequest<TEvent> request,
        Func<Task> next,
        CancellationToken cancellationToken)
        where TObserver : IEventObserver<TEvent>
        where TEvent : class
    {
        try
        {
            await observer.ReceiveAsync(request.Event, cancellationToken);
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
        Func<Task> next,
        CancellationToken cancellationToken)
        where TObserver : IEventObserver<TEvent>
        where TEvent : class
    {
        try
        {
            await observer.ReceiveAsync(request.Event, cancellationToken);
            request.Result.Success();
        }
        catch (Exception ex)
        {
            request.Result.Failure(new EventDeliveryError(typeof(TObserver), ex, ""));
        }

        await next();
    }

    public static void BuildNotifyInCurrentScope<TService, TEvent>(MethodInfo method)
        where TService : class
        where TEvent : class
    {
        // use ObserverMethodResolver, or receive by parameter
    }

    public static async Task NotifyInCurrentScope<TService, TEvent>(
        TService service,
        Func<TService, TEvent, CancellationToken, Task> observer,
        CurrentScopeEventDispatchRequest<TEvent> request,
        Func<Task> next,
        CancellationToken cancellationToken)
        where TEvent : class
    {
        try
        {
            await observer(service, request.Event, cancellationToken);
            request.Result.Success();
        }
        catch (Exception ex)
        {
            request.Result.Failure(new EventDeliveryError(typeof(TService), ex, ""));
        }

        await next();
    }
}

internal class ObserverMethodResolver
{
    public ObserverMethodResolver(MethodInfo method)
    {

    }
}