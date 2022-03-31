using System;

namespace RoyalCode.PipelineFlow
{
    /// <summary>
    /// <para>
    ///     Pipeline Factory for a specific type of pipeline, defined by <typeparamref name="TFor"/>.
    /// </para>
    /// <para>
    ///     It is used for creating pipelines from the request type (input and output types).
    /// </para>
    /// </summary>
    /// <typeparam name="TFor">The specific type of pipeline.</typeparam>
    public interface IPipelineFactory<TFor> : IPipelineFactory { }
    
    /// <summary>
    /// <para>
    ///     A Pipeline Factory.
    /// </para>
    /// <para>
    ///     It is used for creating pipelines from the request type (input and output types).
    /// </para>
    /// </summary>
    public interface IPipelineFactory
    {
        /// <summary>
        /// Create a pipeline for the input type without result.
        /// </summary>
        /// <typeparam name="TIn">The input type.</typeparam>
        /// <returns>A new instance of pipeline.</returns>
        IPipeline<TIn> Create<TIn>();

        /// <summary>
        /// Create a pipeline for the input type with result of output type.
        /// </summary>
        /// <typeparam name="TIn">The input type.</typeparam>
        /// <typeparam name="TOut">The output type.</typeparam>
        /// <returns>A new instance of pipeline.</returns>
        IPipeline<TIn, TOut> Create<TIn, TOut>();

        /// <summary>
        /// Create a pipeline for the input type without result.
        /// </summary>
        /// <param name="inputType">The input type.</param>
        /// <returns>A new instance of pipeline.</returns>
        IPipelineCaller Create(Type inputType);

        /// <summary>
        /// Create a pipeline for the input type with result of output type.
        /// </summary>
        /// <param name="inputType">The input type.</param>
        /// <typeparam name="TOut">The output type.</typeparam>
        /// <returns>A new instance of pipeline.</returns>
        IPipelineCaller<TOut> Create<TOut>(Type inputType);
    }
}
