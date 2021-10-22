using System;

namespace RoyalCode.PipelineFlow.Builders
{
    /// <inheritdoc/>
    public interface IPipelineChainTypeBuilder<TFor> : IPipelineChainTypeBuilder { }

    /// <summary>
    /// <para>
    ///     This is an internal component used to define the type of the chain class 
    ///     from the type of the input and/or output class.
    /// </para>
    /// </summary>
    public interface IPipelineChainTypeBuilder
    {
        /// <summary>
        /// <para>
        ///     Finds out what type of chain class should be used for a pipeline input type.
        /// </para>
        /// <para>
        ///     Given an input type, it will be analyzed and then the chain type that will be used is defined, 
        ///     such that all the handlers in the pipeline of the input type can be executed.
        /// </para>
        /// </summary>
        /// <param name="inputType">The type of the input.</param>
        /// <param name="bridgeChainTypes">Utility for avoiding loops in bridge handlers.</param>
        /// <returns>
        ///     The type of chain class.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Case it is not possible to create a type because no handler was found or a bridge loop occurred.
        /// </exception>
        Type Build(Type inputType, BridgeChainTypes? bridgeChainTypes = null);

        /// <summary>
        /// <para>
        ///     Finds out what type of chain class should be used for a pipeline input and output types.
        /// </para>
        /// <para>
        ///     Given an input type and an output, 
        ///     it will be analyzed and then the chain type that will be used is defined, 
        ///     such that all the handlers in the pipeline of the input type can be executed.
        /// </para>
        /// </summary>
        /// <param name="inputType">The type of input class.</param>
        /// <param name="outputType">The type of output class.</param>
        /// <param name="bridgeChainTypes">Utility for avoiding loops in bridge handlers.</param>
        /// <returns>
        ///     The type of chain class.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Case it is not possible to create a type because no handler was found or a bridge loop occurred.
        /// </exception>
        Type Build(Type inputType, Type outputType, BridgeChainTypes? bridgeChainTypes = null);
    }
}