
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

public class ObserverMethodResolver
{
    public ObserverMethodResolver(MethodInfo method)
    {
        var parameters = method.GetParameters();
        if (parameters.Length == 0)
            throw new InvalidObserverMethodException("Invalid observer method"); // create a better message and resource.


    }
}

public class InvalidObserverMethodException : InvalidOperationException
{
    public InvalidObserverMethodException(string message) : base(message) { }
}