using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// The descriptor of a handler contains information to determine the type of chain that will be used 
    /// and provides the delegate that will be used by the chain.
    /// </summary>
    public interface IHandlerDescriptor
    {
        /// <summary>
        /// <para>
        ///     The input type of the handler, pipeline request.
        /// </para>
        /// <para>
        ///     The input type may be a generic type.
        /// </para>
        /// </summary>
        Type InputType { get; }

        /// <summary>
        /// <para>
        ///     The output type of handler, the pipeline result.
        /// </para>
        /// <para>
        ///     The output type may be a generic type.
        /// </para>
        /// </summary>
        Type OutputType { get; }

        /// <summary>
        /// If the handler of the request (input type) will produce a result.
        /// </summary>
        bool HasOutput { get; }

        /// <summary>
        /// If the handler is async.
        /// </summary>
        bool IsAsync { get; }

        /// <summary>
        /// If the handler is async and has a cancellation token parameter.
        /// </summary>
        bool HasToken { get; }

        /// <summary>
        /// If the handler delegate depends of a service.
        /// </summary>
        Type? ServiceType { get; }

        /// <summary>
        /// Create the delegate handler.
        /// </summary>
        /// <param name="inputType">The actual input type, not generic type.</param>
        /// <param name="outputType">The actual output type, not generic type.</param>
        /// <returns>
        ///     The delegate to handle the pipeline request.
        /// </returns>
        Delegate CreateDelegate(Type inputType, Type outputType);
    }
}
