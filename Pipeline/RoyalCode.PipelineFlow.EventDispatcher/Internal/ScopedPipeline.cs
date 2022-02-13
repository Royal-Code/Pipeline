using System;
using Microsoft.Extensions.DependencyInjection;
using RoyalCode.EventDispatcher;

namespace RoyalCode.PipelineFlow.EventDispatcher.Internal;

/// <summary>
/// <para>
///     Internal class to manage scoped <see cref="IPipeline{TIn}"/> to dispatch events.
/// </para>
/// </summary>
/// <typeparam name="TIn">The input type of the pipeline.</typeparam>
internal sealed class ScopedPipeline<TIn> : IDisposable
{
    private readonly IServiceScope scope;
    private IPipeline<TIn>? pipeline;

    /// <summary>
    /// Creates a new instance with the new service scope.
    /// </summary>
    /// <param name="scope">The new scope.</param>
    public ScopedPipeline(IServiceScope scope)
    {
        this.scope = scope;
    }

    /// <summary>
    /// Gets the scoped pipeline.
    /// </summary>
    public IPipeline<TIn> Pipeline => pipeline ??= scope.ServiceProvider
        .GetRequiredService<IPipelineFactory<IEventDispatcher>>()
        .Create<TIn>();

    /// <summary>
    /// Dispose the scope.
    /// </summary>
    public void Dispose() => scope.Dispose();
}