using RoyalCode.PipelineFlow.Chains;
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
        /// The next handler, if applicable.
        /// </summary>
        public INextHandlerDescriptor? NextHandler { get; }

        /// <summary>
        /// The tipe of the Handler.
        /// </summary>
        public ChainKind HandlerKind { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="inputType">Initial property value.</param>
        /// <param name="outputType">Initial property value.</param>
        /// <param name="hasOutput">Initial property value.</param>
        /// <param name="isAsync">Initial property value.</param>
        /// <param name="hasToken">Initial property value.</param>
        /// <param name="serviceType">Initial property value.</param>
        /// <param name="delegate">Initial property value.</param>
        /// <param name="handlerKind">Initial property value.</param>
        /// <param name="nextHandler">Initial property value.</param>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="inputType"/> or <paramref name="outputType"/> or <paramref name="delegate"/> 
        ///     is null.
        /// </exception>
        public HandlerDescribed(
            Type inputType,
            Type outputType,
            bool hasOutput,
            bool isAsync,
            bool hasToken,
            Type? serviceType,
            Delegate @delegate,
            ChainKind handlerKind,
            INextHandlerDescriptor? nextHandler)
        {
            InputType = inputType ?? throw new ArgumentNullException(nameof(inputType));
            OutputType = outputType ?? throw new ArgumentNullException(nameof(outputType));
            HasOutput = hasOutput;
            IsAsync = isAsync;
            HasToken = hasToken;
            ServiceType = serviceType;
            Delegate = @delegate ?? throw new ArgumentNullException(nameof(@delegate));
            HandlerKind = handlerKind;
            NextHandler = nextHandler;
        }
    }
}
