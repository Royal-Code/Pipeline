using Microsoft.Extensions.DependencyInjection.Extensions;
using RoyalCode.EventDispatcher;
using RoyalCode.PipelineFlow;
using RoyalCode.PipelineFlow.Configurations;
using RoyalCode.PipelineFlow.EventDispatcher.Internal;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class PipelineFlowEventDispatcherServiceCollectionExtensions
{
    /// <summary>
    /// <para>
    ///     Adds an event observer that implements the <see cref="IEventObserver{TEvent}"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TObserver">The observer type.</typeparam>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The same instance of <paramref name="services"/> for chain calls.</returns>
    public static IServiceCollection AddObserver<TObserver, TEvent>(this IServiceCollection services)
        where TObserver : IEventObserver<TEvent>
        where TEvent : class
    {
        services.AddEventDispatcher(subscriber => subscriber.AddObserver<TObserver, TEvent>());
        services.TryAddTransient(typeof(TObserver));
        return services;
    }

    /// <summary>
    /// <para>
    ///     Adds a method that will observer events.
    /// </para>
    /// <para>
    ///     To better understand the method constraints see <see cref="ObserverMethodResolver"/>.
    /// </para>
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="method">The observer method.</param>
    /// <param name="strategy">The dispatch strategy.</param>
    /// <returns>The same instance of <paramref name="services"/> for chain calls.</returns>
    public static IServiceCollection AddObserver(this IServiceCollection services,
        MethodInfo method, DispatchStrategy strategy)
    {
        services.AddEventDispatcher(subscriber => subscriber.AddObserver(method, strategy));
        services.TryAddTransient(method.DeclaringType);
        return services;
    }


    /// <summary>
    /// <para>
    ///     Add the services for <see cref="IEventDispatcher"/> and, optionally, configure the pipeline.
    /// </para>
    /// </summary>
    /// <param name="services">IServiceCollection.</param>
    /// <param name="pipelineConfigureAction">Action to configure the pipeline, optional.</param>
    /// <returns>The same instance of <paramref name="services"/> to chain calls.</returns>
    public static IServiceCollection AddEventDispatcher(this IServiceCollection services,
        Action<ObserverSubscriber>? pipelineConfigureAction = null)
    {
        var configuration = services.GetPipelineFactoryConfiguration();

        if (pipelineConfigureAction is not null)
        {
            configuration.ConfigurePipelines(builder => pipelineConfigureAction(new ObserverSubscriber(builder)));
        }

        return services;
    }

    private static PipelineFactoryConfiguration<IEventDispatcher> GetPipelineFactoryConfiguration(
            this IServiceCollection services)
    {
        return services.GetPipelineFactoryConfiguration<IEventDispatcher>(static (cfg, sc) =>
        {
            //TODO: adds the event dispatcher services ...
            sc.AddTransient<IEventDispatcher, EventDispatcher>();
            sc.AddTransient<IPipelineDispatcherFactory, PipelineDispatcherFactory>();
            sc.AddTransient<EventDispatcherPipelineFactory>();
            sc.AddSingleton<DispatcherStateCollection>();
            cfg.ConfigurePipelines(builder => builder
                .AddHandlerMethodDefined(
                    typeof(CurrentScopeEventDispatchHandler<>),
                    nameof(CurrentScopeEventDispatchHandler<object>.CurrentScopeEventDispatch))
                .AddHandlerMethodDefined(
                    typeof(SeparatedScopeEventDispatchHandler<>),
                    nameof(SeparatedScopeEventDispatchHandler<object>.CurrentScopeEventDispatch)
                ));
        });
    }
}

