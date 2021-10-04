using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    public class BridgeDescriptor : HandlerDescriptor
    {
        private readonly BridgeNextHandlerDescriptor nextHandlerDescription;

        public BridgeDescriptor(
            Type inputType,
            Type outputType,
            Func<Type, Type, Delegate> handlerDelegateProvider,
            BridgeNextHandlerDescriptor nextHandlerDescription)
            : base(inputType, outputType, handlerDelegateProvider)
        {
            this.nextHandlerDescription = nextHandlerDescription;
        }

        public override bool IsBridge => true;

        public override BridgeNextHandlerDescriptor GetBridgeNextHandlerDescription() => nextHandlerDescription;

        public bool HasNextOutput => OutputType != nextHandlerDescription.OutputType;

        public Type NextInputType => nextHandlerDescription.InputType;

        public Type NextOutputType => nextHandlerDescription.OutputType;
    }
}
