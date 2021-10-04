using RoyalCode.PipelineFlow.Configurations;
using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    public class DecoratorDescriptor : DescriptorBase
    {
        public DecoratorDescriptor(Type inputType, Type outputType, Func<Type, Type, Delegate> handlerDelegateProvider)
            : base(inputType, outputType, handlerDelegateProvider)
        { }

        public SortDescriptor SortDescriptor { get; set; } = SortDescriptor.Default;

        public bool Match(Type inputType)
        {
            return InputType.IsGenericType
                ? InputType.GetGenericTypeDefinition() == inputType.GetGenericTypeDefinition() && !HasOutput
                : InputType.IsGenericParameter || InputType == inputType && !HasOutput;
        }

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
