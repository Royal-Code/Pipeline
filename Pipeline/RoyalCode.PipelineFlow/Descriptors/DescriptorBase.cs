using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// A base implementation of <see cref="IHandlerDescriptor"/>.
    /// </summary>
    public class DescriptorBase : IHandlerDescriptor
    {
        /// <inheritdoc/>
        public Type InputType { get; }

        /// <inheritdoc/>
        public Type OutputType { get; }

        /// <inheritdoc/>
        public bool HasOutput { get; internal set; }

        /// <inheritdoc/>
        public bool IsAsync { get; internal set; }

        /// <inheritdoc/>
        public bool HasToken { get; internal set; }

        /// <inheritdoc/>
        public Type? ServiceType { get; internal set; }

        /// <inheritdoc/>
        public bool HasGenericService { get; internal set; }

        /// <inheritdoc/>
        public Func<Type, Type, Delegate> HandlerDelegateProvider { get; }

        /// <summary>
        /// Create a new instance with most base information.
        /// </summary>
        /// <param name="inputType">The handler input type.</param>
        /// <param name="outputType">The handler output type</param>
        /// <param name="handlerDelegateProvider">The factory to generate the handler delegate.</param>
        /// <exception cref="ArgumentNullException">
        ///     If some parameter is null.
        /// </exception>
        public DescriptorBase(Type inputType, Type outputType, Func<Type, Type, Delegate> handlerDelegateProvider)
        {
            InputType = inputType ?? throw new ArgumentNullException(nameof(inputType));
            OutputType = outputType ?? throw new ArgumentNullException(nameof(outputType));
            HandlerDelegateProvider = handlerDelegateProvider ?? throw new ArgumentNullException(nameof(handlerDelegateProvider));
        }

        /// <inheritdoc/>
        public HandlerDescribed Describe(Type inputType, Type outputType)
        {
            var @delegate = HandlerDelegateProvider(inputType, outputType);


            return new HandlerDescribed(inputType, outputType, HasOutput, IsAsync, HasToken, null, @delegate);
        }
    }
}
