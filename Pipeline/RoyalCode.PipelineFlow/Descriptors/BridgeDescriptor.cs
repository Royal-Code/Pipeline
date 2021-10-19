using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// <para>
    ///     The descriptor of a handler contains information to determine the type of chain that will be used 
    ///     and provides the delegate that will be used by the chain.
    /// </para>
    /// <para>
    ///     In this class are bridge descriptors.
    /// </para>
    /// </summary>
    public class BridgeDescriptor : HandlerDescriptor
    {
        private readonly BridgeNextHandlerDescriptor nextHandlerDescription;

        /// <summary>
        /// Create a new instance with most base information.
        /// </summary>
        /// <param name="inputType">The handler input type.</param>
        /// <param name="outputType">The handler output type</param>
        /// <param name="handlerDelegateProvider">The factory to generate the handler delegate.</param>
        /// <param name="nextHandlerDescription">The description for the next handler.</param>
        /// <exception cref="ArgumentNullException">
        ///     If some parameter is null.
        /// </exception>
        public BridgeDescriptor(
            Type inputType,
            Type outputType,
            Func<Type, Type, Delegate> handlerDelegateProvider,
            BridgeNextHandlerDescriptor nextHandlerDescription)
            : base(inputType, outputType, handlerDelegateProvider)
        {
            this.nextHandlerDescription = nextHandlerDescription 
                ?? throw new ArgumentNullException(nameof(nextHandlerDescription));
        }

        /// <inheritdoc/>
        public override bool IsBridge => true;

        /// <inheritdoc/>
        public override BridgeNextHandlerDescriptor GetBridgeNextHandlerDescription() => nextHandlerDescription;

        /// <summary>
        /// Determines whether the next handler produces a result of a different type than the current handler.
        /// </summary>
        public bool HasNextOutput => OutputType != nextHandlerDescription.OutputType;

        /// <summary>
        /// The next handler input type.
        /// </summary>
        public Type NextInputType => nextHandlerDescription.InputType;

        /// <summary>
        /// The next handler output type.
        /// </summary>
        public Type NextOutputType => nextHandlerDescription.OutputType;
    }
}
