using RoyalCode.EventDispatcher;
using RoyalCode.PipelineFlow.Configurations;
using RoyalCode.PipelineFlow.EventDispatcher.Internal;
using RoyalCode.PipelineFlow.Resolvers;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// <para>
///     A class to subscribes event observers.
/// </para>
/// </summary>
public class ObserverSubscriber
{
    private readonly IPipelineBuilder builder;

    internal ObserverSubscriber(IPipelineBuilder builder)
    {
        this.builder = builder;
    }

    /// <summary>
    /// <para>
    ///     Adds an event observer that implements the <see cref="IEventObserver{TEvent}"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TObserver">The observer type.</typeparam>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <returns>The same instance for chain calls.</returns>
    public ObserverSubscriber AddObserver<TObserver, TEvent>()
        where TObserver : IEventObserver<TEvent>
        where TEvent : class
    {
        if (typeof(TObserver).GetDispatchStrategy() == DispatchStrategy.InCurrentScope)
        {
            builder.Configure<CurrentScopeEventDispatchRequest<TEvent>>()
                .WithService<TObserver>()
                .DecorateAsync(NotifyObserverDecoratorHandlers.NotifyInCurrentScope);
        }
        else
        {
            builder.Configure<SeparatedScopeEventDispatchRequest<TEvent>>()
                .WithService<TObserver>()
                .DecorateAsync(NotifyObserverDecoratorHandlers.NotifyInSeparatedScope);
        }
        return this;
    }

    /// <summary>
    /// <para>
    ///     Adds a method that will observer events.
    /// </para>
    /// <para>
    ///     To better understand the method constraints see <see cref="ObserverMethodResolver"/>.
    /// </para>
    /// </summary>
    /// <param name="method">The observer method.</param>
    /// <param name="strategy">The dispatch strategy.</param>
    /// <returns>The same instance for chain calls.</returns>
    public ObserverSubscriber AddObserver(MethodInfo method, DispatchStrategy strategy)
    {
        var @delegate = strategy == DispatchStrategy.InCurrentScope
            ? NotifyObserverDecoratorHandlers.BuildNotifyInCurrentScope(method)
            : NotifyObserverDecoratorHandlers.BuildNotifyInSeparatedScope(method);

        var decorator = new ServiceAndDelegateDecoratorResolver(@delegate, method.DeclaringType);
        builder.AddDecoratorResolver(decorator);

        return this;
    }
}

