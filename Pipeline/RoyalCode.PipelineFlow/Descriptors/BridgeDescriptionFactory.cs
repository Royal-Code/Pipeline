using System;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// <para>
    ///     Internal utilities for creating handler descriptors.
    /// </para>
    /// <para>
    ///     Bridge handlers are produced in this class.
    /// </para>
    /// </summary>
    internal static class BridgeDescriptionFactory
    {
        /// <summary>
        /// Descriptor from a delegate.
        /// </summary>
        /// <param name="bridgeHandler"></param>
        /// <returns></returns>
        internal static BridgeDescriptor GetBridgeDescription(this Delegate bridgeHandler)
        {
            var builder = DescriptorBuilder.Create(bridgeHandler);
            builder.ReadBridgeParameters();
            builder.ReadBridgeNextHandler();
            builder.ValidateBridgeParameters();
            return builder.BuildBridgeDescription();
        }

        /// <summary>
        /// Descriptor from a delegate with a dependent service.
        /// </summary>
        /// <param name="bridgeHandler"></param>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        internal static BridgeDescriptor GetBridgeDescription(this Delegate bridgeHandler, Type serviceType)
        {
            var builder = DescriptorBuilder.Create(bridgeHandler, serviceType);
            builder.ReadBridgeParameters();
            builder.ReadBridgeNextHandler();
            builder.ValidateBridgeParameters();
            return builder.BuildBridgeDescription();
        }

        /// <summary>
        /// Descriptor from a method.
        /// </summary>
        /// <param name="bridgeMethod"></param>
        /// <returns></returns>
        internal static BridgeDescriptor GetBridgeDescription(this MethodInfo bridgeMethod)
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
