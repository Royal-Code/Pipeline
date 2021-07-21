using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal static class BridgeDescriptionFactory
    {
        internal static BridgeDescription GetBridgeDescription(this Delegate bridgeHandler)
        {
            var builder = DescriptionBuilder.Create(bridgeHandler);
            builder.ReadBridgeParameters();
            builder.ReadBridgeNextHandler();
            builder.ValidateBridgeParameters();
            return builder.BuildBridgeDescription();
        }

        internal static BridgeDescription GetBridgeDescription(this Delegate decoratorHandler, Type serviceType)
        {
            var builder = DescriptionBuilder.Create(decoratorHandler, serviceType);
            builder.ReadBridgeParameters();
            builder.ReadBridgeNextHandler();
            builder.ValidateBridgeParameters();
            return builder.BuildBridgeDescription();
        }

        internal static BridgeDescription GetBridgeDescription(this MethodInfo decoratorMethod)
        {
            var builder = DescriptionBuilder.Create(decoratorMethod);
            builder.ReadBridgeParameters();
            builder.ReadBridgeNextHandler();
            builder.ValidateBridgeParameters();
            builder.ResolveMethodHandlerProvider();
            return builder.BuildBridgeDescription();
        }
    }
}
