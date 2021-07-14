using RoyalCode.PipelineFlow.Extensions;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class DecoratorDescription : DescriptionBase
    {
        public DecoratorDescription(Type inputType, Type outputType, Func<Type, Type, Delegate> handlerDelegateProvider) 
            : base(inputType, outputType, handlerDelegateProvider)
        { }

        public bool Match(Type inputType)
        {
            if (HasOutput)
                return false;

            return InputType.IsGenericType
                ? inputType.Implements(InputType.GetGenericTypeDefinition()) && !HasOutput
                : inputType.Implements(InputType) && !HasOutput;
        }

        public bool Match(Type inputType, Type outputType)
        {
            return
                (InputType.IsGenericType
                    ? inputType.Implements(InputType.GetGenericTypeDefinition())
                    : inputType.Implements(InputType))
                && (OutputType.IsGenericType
                    ? outputType.Implements(OutputType.GetGenericTypeDefinition())
                    : outputType.Implements(OutputType));
        }
    }
}
