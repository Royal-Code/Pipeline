using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// A base implementation of <see cref="IHandlerDescriptor"/>.
    /// </summary>
    public class DescriptorBase : IHandlerDescriptor
    {
        public Type InputType { get; }
        public Type OutputType { get; }
        public bool HasOutput { get; internal set; }
        public bool IsAsync { get; internal set; }
        public bool HasToken { get; internal set; }
        public Type? ServiceType { get; internal set; }
        public bool HasGenericService { get; internal set; }

        public Func<Type, Type, Delegate> HandlerDelegateProvider { get; }

        public DescriptorBase(Type inputType, Type outputType, Func<Type, Type, Delegate> handlerDelegateProvider)
        {
            InputType = inputType ?? throw new ArgumentNullException(nameof(inputType));
            OutputType = outputType ?? throw new ArgumentNullException(nameof(outputType));
            HandlerDelegateProvider = handlerDelegateProvider ?? throw new ArgumentNullException(nameof(handlerDelegateProvider));
        }

        public Delegate CreateDelegate(Type inputType, Type outputType)
            => HandlerDelegateProvider(inputType, outputType);
    }
}
