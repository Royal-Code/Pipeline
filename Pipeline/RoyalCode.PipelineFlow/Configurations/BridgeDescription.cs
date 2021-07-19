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
    }

    internal static class BridgeDescriptionFactory
    {
        internal static BridgeDescription GetDecoratorDescription(this Delegate bridgeHandler)
        {
            var builder = DescriptionBuilder.Create(bridgeHandler);
            builder.ReadBridgeParameters();
            builder.ReadBridgeNextHandler();
            // validate parameters
            // build description

            builder.ValidateDecoratorParameters();

            throw new NotImplementedException();
        }
    }
}
