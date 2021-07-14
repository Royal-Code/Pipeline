using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class HandlerDescription : DescriptionBase
    {
        public HandlerDescription(Type inputType, Type outputType, Func<Type, Type, Delegate> handlerDelegateProvider) 
            : base(inputType, outputType, handlerDelegateProvider)
        { }

        public bool IsBridge { get; internal set; }

        public Type GetBridgeType()
        {
            throw new NotImplementedException();
        }

        public bool Match(Type inputType)
        {
            return InputType.IsGenericType
                ? InputType.GetGenericTypeDefinition() == inputType.GetGenericTypeDefinition() && !HasOutput
                : InputType == inputType && !HasOutput;
        }

        public bool Match(Type inputType, Type outputType)
        {
            return 
                (InputType.IsGenericType
                    ? InputType.GetGenericTypeDefinition() == inputType.GetGenericTypeDefinition()
                    : InputType == inputType)
                && (OutputType.IsGenericType
                    ? OutputType.GetGenericTypeDefinition() == outputType.GetGenericTypeDefinition()
                    : OutputType == outputType);
        }
    }
}
