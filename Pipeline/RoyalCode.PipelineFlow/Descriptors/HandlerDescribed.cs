using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// <para>
    ///     It contains basically the same information as the <see cref="IHandlerDescriptor"/>, adding the delegate.
    /// </para>
    /// <para>
    ///     When the handler is from a service, the service type will not be generic,
    ///     whereas in the descriptor the service type can be generic.
    /// </para>
    /// </summary>
    public class HandlerDescribed 
    {
        /// <summary>
        /// <para>
        ///     The input type of the handler, pipeline request.
        /// </para>
        /// </summary>
        public Type InputType { get; }

        /// <summary>
        /// <para>
        ///     The output type of handler, the pipeline result.
        /// </para>
        /// </summary>
        public Type OutputType { get; }

        /// <summary>
        /// If the handler of the request (input type) will produce a result.
        /// </summary>
        public bool HasOutput { get; }

        /// <summary>
        /// If the handler is async.
        /// </summary>
        public bool IsAsync { get; }

        /// <summary>
        /// If the handler is async and has a cancellation token parameter.
        /// </summary>
        public bool HasToken { get; }

        /// <summary>
        /// If the handler delegate depends of a service.
        /// </summary>
        public Type? ServiceType { get; }

        /// <summary>
        /// The delegate handler.
        /// </summary>
        public Delegate Delegate { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="outputType"></param>
        /// <param name="hasOutput"></param>
        /// <param name="isAsync"></param>
        /// <param name="hasToken"></param>
        /// <param name="serviceType"></param>
        /// <param name="delegate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public HandlerDescribed(
            Type inputType,
            Type outputType,
            bool hasOutput,
            bool isAsync,
            bool hasToken,
            Type? serviceType,
            Delegate @delegate)
        {
            InputType = inputType ?? throw new ArgumentNullException(nameof(inputType));
            OutputType = outputType ?? throw new ArgumentNullException(nameof(outputType));
            HasOutput = hasOutput;
            IsAsync = isAsync;
            HasToken = hasToken;
            ServiceType = serviceType;
            Delegate = @delegate ?? throw new ArgumentNullException(nameof(@delegate));
        }
    }
}
