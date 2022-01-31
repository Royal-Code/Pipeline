using RoyalCode.EventDispatcher;
using RoyalCode.PipelineFlow;
using RoyalCode.PipelineFlow.Configurations;
using RoyalCode.PipelineFlow.EventDispatcher.Internal;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class PipelineFlowEventDispatcherServiceCollectionExtensions
{

    public static void AddObserver<TObserver, TEvent>(this IServiceCollection services)
        where TObserver : IEventObserver<TEvent>
        where TEvent : class
    {
        services.AddEventDispatcher(builder =>
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
        });
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
        Action<IPipelineBuilder>? pipelineConfigureAction = null)
    {
        var configuration = services.GetPipelineFactoryConfiguration();

        if (pipelineConfigureAction is not null)
        {
            configuration.ConfigurePipelines(pipelineConfigureAction);
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

