using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Resolvers
{
    /// <summary>
    /// <para>
    ///     The resolver component is used to provide descriptors for handlers when they are able to process the pipeline request.
    /// </para>
    /// <para>
    ///     Resolvers that implement this interface are for decorators handlers.
    /// </para>
    /// </summary>
    public interface IDecoratorResolver
    {
        /// <summary>
        /// For the input type, try solving a handler that doesn't produce results.
        /// </summary>
        /// <param name="inputType">The pipeline input type.</param>
        /// <returns>
        ///     A decorator handler description, or null if can't be applied.
        /// </returns>
        DecoratorDescriptor? TryResolve(Type inputType);

        /// <summary>
        /// For the input type, try solving a handler that produces a results of output type.
        /// </summary>
        /// <param name="inputType">The pipeline input type.</param>
        /// <param name="outputType">The pipeline output type.</param>
        /// <returns>
        ///     A decorator handler description, or null if can't be applied.
        /// </returns>
        DecoratorDescriptor? TryResolve(Type inputType, Type outputType);
    }
}
