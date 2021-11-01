using RoyalCode.PipelineFlow.Chains;
using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// <para>
    ///     The descriptor of a handler contains information to determine the type of chain that will be used 
    ///     and provides the delegate that will be used by the chain.
    /// </para>
    /// </summary>
    public class HandlerDescriptor : DescriptorBase
    {
        /// <inheritdoc/>
        public HandlerDescriptor(Type inputType, Type outputType, Func<Type, Type, Delegate> handlerDelegateProvider)
            : base(inputType, outputType, handlerDelegateProvider)
        { }

        /// <summary>
        /// If the handler is a bridge handle.
        /// </summary>
        public virtual bool IsBridge => false;

        /// <inheritdoc/>
        protected override ChainKind HandlerKind => ChainKind.Handler;

        /// <inheritdoc/>
        protected override INextHandlerDescriptor? NextHandlerDescriptor => null;

        /// <summary>
        /// <para>
        ///     Get the descriptor for next handler.
        /// </para>
        /// <para>
        ///     It will be allowed only if <see cref="IsBridge"/> is true.
        /// </para>
        /// </summary>
        /// <returns>
        ///     The description for the handler (<see cref="BridgeNextHandlerDescriptor"/>).
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If the handler is not a bridge (<see cref="IsBridge"/> is false.).
        /// </exception>
        public virtual BridgeNextHandlerDescriptor GetBridgeNextHandlerDescription()
            => throw new InvalidOperationException("This handler description is not a bridge handler.");

        /// <summary>
        /// Checks if the handler of this descriptor can handle the input type request.
        /// </summary>
        /// <param name="inputType">The request input type.</param>
        /// <returns>
        ///     True if can handle, false otherwise.
        /// </returns>
        public bool Match(Type inputType)
        {
            return InputType.IsGenericType
                ? InputType.GetGenericTypeDefinition() == inputType.GetGenericTypeDefinition() && !HasOutput
                : InputType.IsGenericParameter || InputType == inputType && !HasOutput;
        }

        /// <summary>
        /// Checks if the handler of this descriptor can handle the request input type and produce the result output type.
        /// </summary>
        /// <param name="inputType">The request input type.</param>
        /// <param name="outputType">The result output type.</param>
        /// <returns>
        ///     True if can handle, false otherwise.
        /// </returns>
        public bool Match(Type inputType, Type outputType)
        {
            return
                (InputType.IsGenericType
                    ? InputType.GetGenericTypeDefinition() == inputType.GetGenericTypeDefinition()
                    : InputType.IsGenericParameter || InputType == inputType)
                && (OutputType.IsGenericType
                    ? OutputType.GetGenericTypeDefinition() == outputType.GetGenericTypeDefinition()
                    : OutputType.IsGenericParameter || OutputType == outputType);
        }
    }
}
