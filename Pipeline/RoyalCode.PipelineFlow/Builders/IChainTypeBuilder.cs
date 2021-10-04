using System;
using RoyalCode.PipelineFlow.Chains;
using RoyalCode.PipelineFlow.Descriptors;

namespace RoyalCode.PipelineFlow.Builders
{
    /// <summary>
    /// <para>
    ///     This builder is responsible for choosing the chain type 
    ///     (<see cref="Chain{TIn}"/>, <see cref="Chain{TIn, TOut}"/>)
    ///     that will mediate the call to the handler.
    /// </para>
    /// <para>
    ///     There are a wide variety of chain implementations, 
    ///     which mediate the delegation of processing between the various handlers in the pipeline.
    /// </para>
    /// <para>
    ///     This builder will determine an implementation that meets the requirements of the handler description.
    /// </para>
    /// </summary>
    public interface IChainTypeBuilder
    {
        /// <summary>
        /// The kind of handler that this builder can process.
        /// </summary>
        ChainKind Kind { get; }

        /// <summary>
        /// Determines the type of chain that will mediate the handler call,
        /// from the handler description and, if it exists, the chain to the next handler.
        /// </summary>
        /// <param name="descriptor">The handler descriptor.</param>
        /// <param name="previousChainType">The chain for the next handler.</param>
        /// <returns>The chain type (<see cref="Chain{TIn}"/>, <see cref="Chain{TIn, TOut}"/>).</returns>
        Type Build(IHandlerDescriptor descriptor, Type? previousChainType = null);
    }
}
