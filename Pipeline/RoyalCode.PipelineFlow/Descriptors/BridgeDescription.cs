using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class BridgeDescription : HandlerDescription
    {
        private readonly BridgeNextHandlerDescription nextHandlerDescription;

        public BridgeDescription(
            Type inputType,
            Type outputType,
            Func<Type, Type, Delegate> handlerDelegateProvider,
            BridgeNextHandlerDescription nextHandlerDescription)
            : base(inputType, outputType, handlerDelegateProvider)
        {
            this.nextHandlerDescription = nextHandlerDescription;
        }

        public override bool IsBridge => true;

        public override BridgeNextHandlerDescription GetBridgeNextHandlerDescription() => nextHandlerDescription;

        public bool HasNextOutput => OutputType != nextHandlerDescription.OutputType;

        public Type NextInputType => nextHandlerDescription.InputType;

        public Type NextOutputType => nextHandlerDescription.OutputType;
    }
}
