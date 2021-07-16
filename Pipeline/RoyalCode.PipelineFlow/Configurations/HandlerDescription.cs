﻿using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class HandlerDescription : DescriptionBase
    {
        public HandlerDescription(Type inputType, Type outputType, Func<Type, Type, Delegate> handlerDelegateProvider) 
            : base(inputType, outputType, handlerDelegateProvider)
        { }

        public virtual bool IsBridge => false;

        public virtual BridgeNextHandlerDescription GetBridgeNextHandlerDescription() 
            => throw new InvalidOperationException("This handler description is not a bridge handler.");
        

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
