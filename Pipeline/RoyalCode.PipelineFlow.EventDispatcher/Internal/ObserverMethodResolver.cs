using RoyalCode.PipelineFlow.EventDispatcher.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     This class is responsible for parsing a method that should be an event handler, an observer.
/// </para>
/// <para>
///     A valida method requires:
///     <list type="bullet">
///         <item>
///             A method whose return is void or Task, and no other type.
///         </item>
///         <item>
///             The first parameter is the event and its type must be a class, 
///             i.e. value objects and primitive types are not accepted.
///         </item>
///         <item>
///             If the method is asynchronous, which returns Task, 
///             it may or may not have the second parameter of <see cref="CancellationToken"/> type.
///         </item>
///         <item>
///             Others parameters are not accepted.
///         </item>
///     </list>
/// </para>
/// <para>
///     This class is responsible for performing these validations and creating a function to execute the method.
/// </para>
/// </summary>
public class ObserverMethodResolver
{
    private readonly MethodInfo method;
    private readonly bool isAsync;
    private readonly Type eventType;
    private readonly bool hasToken;

    /// <summary>
    /// Creates a new resolver.
    /// </summary>
    /// <param name="method">The event observer method.</param>
    /// <exception cref="InvalidObserverMethodException">
    ///     If it is a invalid method.
    /// </exception>
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

        isAsync = IsAsync();
        eventType = parameters[0].ParameterType;

        if (!eventType.IsClass)
            throw new InvalidObserverMethodException("The event type must be a class");

        if (parameters.Length == 2)
        {
            if (!IsAsync())
                throw new InvalidObserverMethodException("Invalid observer method, to have two parameters, the method must be async.");

            if (parameters[1].ParameterType != typeof(CancellationToken))
                throw new InvalidObserverMethodException("Invalid observer method, the second parameter must be a cancellation token.");

            hasToken = true;
        }
    }

    /// <summary>
    /// The event Type.
    /// </summary>
    public Type EventType => eventType;

    /// <summary>
    /// <para>
    ///     Creates the function to execute the method.
    /// </para>
    /// <para>
    ///     The function will be: Func{TService, TEvent, CancellationToken, Task},
    ///     where the TService is the method declaring type.
    /// </para>
    /// </summary>
    /// <returns></returns>
    public Delegate BuildCallerFunction()
    {
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

        var funcType = GetFunctionType();

        var lambda = Expression.Lambda(funcType, body, serviceVar, eventVar, tokenVar);

        return lambda.Compile();
    }

    /// <summary>
    /// <para>
    ///     Get the delegate function type that call the event observer method.
    /// </para>
    /// </summary>
    /// <returns>
    ///     A type like Func{TService, TEvent, CancellationToken, Task},
    ///     where the TService is the method declaring type.
    /// </returns>
    public Type GetFunctionType()
    {
        return typeof(Func<,,,>)
            .MakeGenericType(method.DeclaringType, eventType, typeof(CancellationToken), typeof(Task));
    }

    /// <summary>
    /// Check if the method is async or not.
    /// </summary>
    /// <returns>True if the method returns <see cref="Task"/>, false if is void.</returns>
    /// <exception cref="InvalidObserverMethodException">
    ///     If the method return any other type.
    /// </exception>
    public bool IsAsync()
    {
        return method.ReturnType == typeof(Task)
            ? true
            : method.ReturnType == typeof(void)
                ? false
                : throw new InvalidObserverMethodException("Invalid observer method, the return type must be void or Task.");
    }
}
