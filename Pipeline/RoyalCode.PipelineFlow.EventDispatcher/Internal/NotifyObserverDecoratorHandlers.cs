
using RoyalCode.EventDispatcher;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

    public static void BuildNotifyInCurrentScope(MethodInfo method)
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
    private readonly MethodInfo method;
    private readonly bool isAsync;
    private readonly Type eventType;
    private readonly bool hasToken;

    public ObserverMethodResolver(MethodInfo method)
    {
        var parameters = method.GetParameters();
        if (parameters.Length == 0)
            throw new InvalidObserverMethodException(
                "Invalid observer method, it must have one parameter at least."); // create a better message and resource.
        if (parameters.Length > 2)
            throw new InvalidObserverMethodException(
                "Invalid observer method, it must have one or two parameter, where the second parameter must be a cancellation token.");

        this.method = method;

        isAsync = ResolveIsAsync();
        eventType = parameters[0].ParameterType;

        if (!eventType.IsClass)
            throw new InvalidObserverMethodException("The event type must be a class");

        if (parameters.Length == 2)
        {
            if (!isAsync)
                throw new InvalidObserverMethodException("Invalid observer method, to have two parameters, the method must be async.");

            if (parameters[1].ParameterType != typeof(CancellationToken))
                throw new InvalidObserverMethodException("Invalid observer method, the second parameter must be a cancellation token.");

            hasToken = true;
        }
    }

    public Delegate BuildCallerFunction()
    {
        // Func<TService, TEvent, CancellationToken, Task>

        var serviceVar = Expression.Parameter(method.DeclaringType, "service");
        var eventVar = Expression.Parameter(eventType, "event");
        var tokenVar = Expression.Parameter(typeof(CancellationToken), "token");

        var arguments = new LinkedList<Expression>();
        arguments.AddLast(eventVar);
        if (hasToken)
            arguments.AddLast(tokenVar);

        var call = Expression.Call(serviceVar, method, arguments);

        Expression body;
        if (isAsync)
            body = call;
        else
            body = Expression.Block(call, Expression.Constant(Task.CompletedTask));

        //var funcType = typeof(Func<,,,>)
        //    .MakeGenericType(method.DeclaringType, eventType, typeof(CancellationToken), typeof(Task));

        var lambda = Expression.Lambda(body, serviceVar, eventVar, tokenVar);

        return lambda.Compile();
    }

    private bool ResolveIsAsync()
    {
        return method.ReturnType == typeof(Task)
            ? true
            : method.ReturnType == typeof(void)
                ? false
                : throw new InvalidObserverMethodException("Invalid observer method, the return type must be void or Task.");
    }
}

public class InvalidObserverMethodException : InvalidOperationException
{
    public InvalidObserverMethodException(string message) : base(message) { }
}