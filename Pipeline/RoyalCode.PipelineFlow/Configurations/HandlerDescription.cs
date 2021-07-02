using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class HandlerDescription
    {
        public Type InputType { get; internal set; }
        public Type OutputType { get; internal set; }
        public bool HasOutput { get; internal set; }
        public bool IsAsync { get; internal set; }
        public bool HasToken { get; internal set; }

        public Func<Type, Type, Delegate> HandlerDelegateProvider { get; internal set; }

        public Type ServiceType { get; internal set; }

        public bool IsBridge { get; internal set; }

        public bool HasGenericService { get; internal set; }

        public Type GetBridgeType()
        {
            throw new NotImplementedException();
        }

        public bool Match(Type inputType)
        {
            if (HasGenericService)
            {
                throw new NotImplementedException();
            }
            else
            {
                return InputType == inputType && !HasOutput;
            }
        }

        public bool Match(Type inputType, Type outputType)
        {
            if (HasGenericService)
            {
                throw new NotImplementedException();
            }
            else
            {
                return InputType == inputType && OutputType == outputType;
            }
        }
    }
}
