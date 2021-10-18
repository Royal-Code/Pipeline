using RoyalCode.PipelineFlow.Chains;
using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Builders
{
    /// <summary>
    /// <para>
    ///     This implementation of <see cref="IChainTypeBuilder"/> handles the creation of chain types for bridge handler descriptors.
    /// </para>
    /// </summary>
    internal class BridgeChainTypeBuilder: IChainTypeBuilder
    {
        /// <inheritdoc/>
        public ChainKind Kind => ChainKind.Bridge;

        /// <inheritdoc/>
        public Type Build(IHandlerDescriptor descriptor, Type? previousChainType)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            if (previousChainType is null)
                throw new ArgumentNullException(nameof(previousChainType));

            if (descriptor is not BridgeDescriptor description)
                throw new InvalidOperationException(
                    $"{nameof(BridgeChainTypeBuilder)} only accepts {nameof(BridgeDescriptor)}" +
                    $" and the current instance is type of {descriptor.GetType().Name}");

            if (description.ServiceType is null)
            {
                if (description.IsAsync)
                {
                    if (description.HasToken)
                    {
                        if (description.HasOutput)
                        {
                            if (description.HasNextOutput)
                            {
                                return typeof(BridgeChainDelegateAsync<,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    description.NextInputType, description.NextOutputType, previousChainType);
                            }
                            else
                            {
                                return typeof(BridgeChainDelegateAsync<,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    description.NextInputType, previousChainType);
                            }
                        }
                        else
                        {
                            return typeof(BridgeChainDelegateAsync<,,>).MakeGenericType(
                                description.InputType,
                                description.NextInputType, previousChainType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            if (description.HasNextOutput)
                            {
                                return typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    description.NextInputType, description.NextOutputType, previousChainType);
                            }
                            else
                            {
                                return typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    description.NextInputType, previousChainType);
                            }
                        }
                        else
                        {
                            return typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,>).MakeGenericType(
                                description.InputType,
                                description.NextInputType, previousChainType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        if (description.HasNextOutput)
                        {
                            return typeof(BridgeChainDelegateSync<,,,,>).MakeGenericType(
                                description.InputType, description.OutputType,
                                description.NextInputType, description.NextOutputType, previousChainType);
                        }
                        else
                        {
                            return typeof(BridgeChainDelegateSync<,,,>).MakeGenericType(
                                description.InputType, description.OutputType,
                                description.NextInputType, previousChainType);
                        }
                    }
                    else
                    {
                        return typeof(BridgeChainDelegateSync<,,>).MakeGenericType(
                            description.InputType,
                            description.NextInputType, previousChainType);
                    }
                }
            }
            else
            {
                if (description.IsAsync)
                {
                    if (description.HasToken)
                    {
                        if (description.HasOutput)
                        {
                            if (description.HasNextOutput)
                            {
                                return typeof(BridgeChainServiceAsync<,,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    description.NextInputType, description.NextOutputType, previousChainType,
                                    description.ServiceType);
                            }
                            else
                            {
                                return typeof(BridgeChainServiceAsync<,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    description.NextInputType, previousChainType, 
                                    description.ServiceType);
                            }
                        }
                        else
                        {
                            return typeof(BridgeChainServiceAsync<,,,>).MakeGenericType(
                                description.InputType,
                                description.NextInputType, previousChainType,
                                description.ServiceType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            if (description.HasNextOutput)
                            {
                                return typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    description.NextInputType, description.NextOutputType, previousChainType,
                                    description.ServiceType);
                            }
                            else
                            {
                                return typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    description.NextInputType, previousChainType,
                                    description.ServiceType);
                            }
                        }
                        else
                        {
                            return typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,>).MakeGenericType(
                                description.InputType,
                                description.NextInputType, previousChainType,
                                description.ServiceType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        if (description.HasNextOutput)
                        {
                            return typeof(BridgeChainServiceSync<,,,,,>).MakeGenericType(
                                description.InputType, description.OutputType,
                                description.NextInputType, description.NextOutputType, previousChainType,
                                description.ServiceType);
                        }
                        else
                        {
                            return typeof(BridgeChainServiceSync<,,,,>).MakeGenericType(
                                description.InputType, description.OutputType,
                                description.NextInputType, previousChainType,
                                description.ServiceType);
                        }
                    }
                    else
                    {
                        return typeof(BridgeChainServiceSync<,,,>).MakeGenericType(
                            description.InputType,
                            description.NextInputType, previousChainType,
                            description.ServiceType);
                    }
                }
            }
        }
    }
}
