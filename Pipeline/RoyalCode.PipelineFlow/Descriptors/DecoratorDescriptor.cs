using RoyalCode.PipelineFlow.Chains;
using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// <para>
    ///     The descriptor of a handler contains information to determine the type of chain that will be used 
    ///     and provides the delegate that will be used by the chain.
    /// </para>
    /// <para>
    ///     This class describes a decorator handler.
    /// </para>
    /// </summary>
    public class DecoratorDescriptor : DescriptorBase
    {
        /// <inheritdoc/>
        public DecoratorDescriptor(Type inputType, Type outputType, Func<Type, Type, Delegate> handlerDelegateProvider)
            : base(inputType, outputType, handlerDelegateProvider)
        { }

        /// <summary>
        /// Describes how this decorator handler will be ordered among all decorators in the pipeline.
        /// </summary>
        public SortDescriptor SortDescriptor { get; set; } = SortDescriptor.Default;

        /// <inheritdoc/>
        protected override ChainKind HandlerKind => ChainKind.Decorator;

        /// <inheritdoc/>
        protected override INextHandlerDescriptor? NextHandlerDescriptor => null;

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
