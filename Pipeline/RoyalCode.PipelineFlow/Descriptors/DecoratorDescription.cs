using RoyalCode.PipelineFlow.Extensions;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class DecoratorDescription : DescriptorBase
    {
        public DecoratorDescription(Type inputType, Type outputType, Func<Type, Type, Delegate> handlerDelegateProvider) 
            : base(inputType, outputType, handlerDelegateProvider)
        { }

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
