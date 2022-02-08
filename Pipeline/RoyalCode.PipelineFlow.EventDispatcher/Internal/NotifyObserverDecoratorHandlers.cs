
using RoyalCode.EventDispatcher;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

internal static class NotifyObserverDecoratorHandlers
{
    private static MethodInfo NotifyServiceInCurrentScopeMethod = typeof(NotifyObserverDecoratorHandlers)
        .GetMethods()
        .Where(m => m.Name == nameof(NotifyServiceInCurrentScope))
        .First();
    
    private static MethodInfo NotifyServiceInSeparatedScopeMethod = typeof(NotifyObserverDecoratorHandlers)
        .GetMethods()
        .Where(m => m.Name == nameof(NotifyServiceInSeparatedScope))
        .First();

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

    public static Delegate BuildNotifyInCurrentScope(MethodInfo method)
    {
        var resolver = new ObserverMethodResolver(method);
        var observer = resolver.BuildCallerFunction();
        var observerFunctionType = resolver.GetFunctionType();

        var notifyMethod = NotifyServiceInCurrentScopeMethod
            .MakeGenericMethod(method.DeclaringType, resolver.EventType);

        var serviceVar = Expression.Parameter(method.DeclaringType, "service");
        var observerVar = Expression.Constant(observer, observerFunctionType);
        var requestVar = Expression.Parameter(
            typeof(CurrentScopeEventDispatchRequest<>).MakeGenericType(resolver.EventType), "request");
        var nextVar = Expression.Parameter(typeof(Func<Task>), "next");
        var tokenVar = Expression.Parameter(typeof(CancellationToken), "token");

        var call = Expression.Call(notifyMethod, serviceVar, observerVar, requestVar, nextVar, tokenVar);

        //Func<TService, TInput, Func<Task>, CancellationToken, Task>
        var delegateType = typeof(Func<,,,,>).MakeGenericType(
            method.DeclaringType,
            typeof(CurrentScopeEventDispatchRequest<>).MakeGenericType(resolver.EventType),
            typeof(Func<Task>),
            typeof(CancellationToken),
            typeof(Task));

        var lambda = Expression.Lambda(delegateType, call, serviceVar, requestVar, nextVar, tokenVar);
        return lambda.Compile();
    }

    public static Delegate BuildNotifyInSeparatedScope(MethodInfo method)
    {
        var resolver = new ObserverMethodResolver(method);
        var observer = resolver.BuildCallerFunction();
        var observerFunctionType = resolver.GetFunctionType();

        var notifyMethod = NotifyServiceInSeparatedScopeMethod
            .MakeGenericMethod(method.DeclaringType, resolver.EventType);

        var serviceVar = Expression.Parameter(method.DeclaringType, "service");
        var observerVar = Expression.Constant(observer, observerFunctionType);
        var requestVar = Expression.Parameter(
            typeof(SeparatedScopeEventDispatchRequest<>).MakeGenericType(resolver.EventType), "request");
        var nextVar = Expression.Parameter(typeof(Func<Task>), "next");
        var tokenVar = Expression.Parameter(typeof(CancellationToken), "token");

        var call = Expression.Call(notifyMethod, serviceVar, observerVar, requestVar, nextVar, tokenVar);

        //Func<TService, TInput, Func<Task>, CancellationToken, Task>
        var delegateType = typeof(Func<,,,,>).MakeGenericType(
            method.DeclaringType,
            typeof(SeparatedScopeEventDispatchRequest<>).MakeGenericType(resolver.EventType),
            typeof(Func<Task>),
            typeof(CancellationToken),
            typeof(Task));

        var lambda = Expression.Lambda(delegateType, call, serviceVar, requestVar, nextVar, tokenVar);
        return lambda.Compile();
    }

    public static async Task NotifyServiceInCurrentScope<TService, TEvent>(
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

    public static async Task NotifyServiceInSeparatedScope<TService, TEvent>(
        TService service,
        Func<TService, TEvent, CancellationToken, Task> observer,
        SeparatedScopeEventDispatchRequest<TEvent> request,
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
