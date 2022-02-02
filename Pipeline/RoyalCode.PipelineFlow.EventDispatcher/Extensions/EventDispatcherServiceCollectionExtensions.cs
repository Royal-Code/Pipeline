using RoyalCode.EventDispatcher;
using RoyalCode.PipelineFlow;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class PipelineFlowEventDispatcherServiceCollectionExtensions
{
    public static IServiceCollection AddObserver<TObserver, TEvent>(this IServiceCollection services)
        where TObserver : IEventObserver<TEvent>
        where TEvent : class
    {
        services.AddEventDispatcher(subscriber => subscriber.AddObserver<TObserver, TEvent>());
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
        });
    }
}

