using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IDecoratorResolver
    {
        DecoratorDescription? TryResolve(Type inputType);

        DecoratorDescription? TryResolve(Type inputType, Type output);
    }

    public class DecoratorDescription : DescriptionBase
    {
        public DecoratorDescription(Type inputType, Type outputType, Func<Type, Type, Delegate> handlerDelegateProvider) 
            : base(inputType, outputType, handlerDelegateProvider)
        { }

        public bool Match(Type inputType)
        {
            if (HasOutput)
                return false;

            return HasGenericService && InputType.IsGenericType && inputType.IsGenericType
                ? InputType.GetGenericTypeDefinition() == inputType.GetGenericTypeDefinition() && !HasOutput
                : InputType == inputType && !HasOutput;
        }

        public bool Match(Type inputType, Type outputType)
        {
            return
                (InputType.IsGenericType && inputType.IsGenericType
                    ? InputType.GetGenericTypeDefinition() == inputType.GetGenericTypeDefinition()
                    : InputType == inputType)
                && (OutputType.IsGenericType && outputType.IsGenericType
                    ? OutputType.GetGenericTypeDefinition() == outputType.GetGenericTypeDefinition()
                    : OutputType == outputType);
        }
    }
}
