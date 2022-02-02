using RoyalCode.EventDispatcher;
using RoyalCode.PipelineFlow.Configurations;
using RoyalCode.PipelineFlow.EventDispatcher.Internal;
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

    public ObserverSubscriber AddObserver(MethodInfo method, DispatchStrategy strategy)
    {
        if (strategy == DispatchStrategy.InCurrentScope)
        {

        }

        return this;
    }
}

