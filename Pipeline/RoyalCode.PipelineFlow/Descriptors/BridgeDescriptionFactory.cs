using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal static class BridgeDescriptionFactory
    {
        internal static BridgeDescription GetBridgeDescription(this Delegate bridgeHandler)
        {
            var builder = DescriptorBuilder.Create(bridgeHandler);
            builder.ReadBridgeParameters();
            builder.ReadBridgeNextHandler();
            builder.ValidateBridgeParameters();
            return builder.BuildBridgeDescription();
        }

        internal static BridgeDescription GetBridgeDescription(this Delegate bridgeHandler, Type serviceType)
        {
            var builder = DescriptorBuilder.Create(bridgeHandler, serviceType);
            builder.ReadBridgeParameters();
            builder.ReadBridgeNextHandler();
            builder.ValidateBridgeParameters();
            return builder.BuildBridgeDescription();
        }

        internal static BridgeDescription GetBridgeDescription(this MethodInfo bridgeMethod)
        {
            var builder = DescriptorBuilder.Create(bridgeMethod);
            builder.ReadBridgeParameters();
            builder.ReadBridgeNextHandler();
            builder.ValidateBridgeParameters();
            builder.ResolveMethodHandlerProvider();
            return builder.BuildBridgeDescription();
        }
    }
}
