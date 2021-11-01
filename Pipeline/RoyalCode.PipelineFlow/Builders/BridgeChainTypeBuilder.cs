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
        public Type Build(HandlerDescribed handlerDescribed, Type? previousChainType)
        {
            if (handlerDescribed is null)
                throw new ArgumentNullException(nameof(handlerDescribed));

            if (previousChainType is null)
                throw new ArgumentNullException(nameof(previousChainType));

            if (handlerDescribed.HandlerKind is not ChainKind.Bridge)
                throw new InvalidOperationException(
                    $"{nameof(BridgeChainTypeBuilder)} only accepts {nameof(BridgeDescriptor)}" +
                    $" and the current handler kind is {handlerDescribed.HandlerKind}");

            var description = handlerDescribed;
            var nextHandlerDescriptor = handlerDescribed.NextHandler;

            if (nextHandlerDescriptor is null)
                throw new InvalidOperationException(
                    $"{nameof(BridgeChainTypeBuilder)} requires the next handler description and the current " +
                    $"{nameof(HandlerDescribed)} does not provides one.");

            if (description.ServiceType is null)
            {
                if (description.IsAsync)
                {
                    if (description.HasToken)
                    {
                        if (description.HasOutput)
                        {
                            if (nextHandlerDescriptor.HasOutput)
                            {
                                return typeof(BridgeChainDelegateAsync<,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    nextHandlerDescriptor.InputType, nextHandlerDescriptor.OutputType, previousChainType);
                            }
                            else
                            {
                                return typeof(BridgeChainDelegateAsync<,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    nextHandlerDescriptor.InputType, previousChainType);
                            }
                        }
                        else
                        {
                            return typeof(BridgeChainDelegateAsync<,,>).MakeGenericType(
                                description.InputType,
                                nextHandlerDescriptor.InputType, previousChainType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            if (nextHandlerDescriptor.HasOutput)
                            {
                                return typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    nextHandlerDescriptor.InputType, nextHandlerDescriptor.OutputType, previousChainType);
                            }
                            else
                            {
                                return typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    nextHandlerDescriptor.InputType, previousChainType);
                            }
                        }
                        else
                        {
                            return typeof(BridgeChainDelegateWithoutCancellationTokenAsync<,,>).MakeGenericType(
                                description.InputType,
                                nextHandlerDescriptor.InputType, previousChainType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        if (nextHandlerDescriptor.HasOutput)
                        {
                            return typeof(BridgeChainDelegateSync<,,,,>).MakeGenericType(
                                description.InputType, description.OutputType,
                                nextHandlerDescriptor.InputType, nextHandlerDescriptor.OutputType, previousChainType);
                        }
                        else
                        {
                            return typeof(BridgeChainDelegateSync<,,,>).MakeGenericType(
                                description.InputType, description.OutputType,
                                nextHandlerDescriptor.InputType, previousChainType);
                        }
                    }
                    else
                    {
                        return typeof(BridgeChainDelegateSync<,,>).MakeGenericType(
                            description.InputType,
                            nextHandlerDescriptor.InputType, previousChainType);
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
                            if (nextHandlerDescriptor.HasOutput)
                            {
                                return typeof(BridgeChainServiceAsync<,,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    nextHandlerDescriptor.InputType, nextHandlerDescriptor.OutputType, previousChainType,
                                    description.ServiceType);
                            }
                            else
                            {
                                return typeof(BridgeChainServiceAsync<,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    nextHandlerDescriptor.InputType, previousChainType, 
                                    description.ServiceType);
                            }
                        }
                        else
                        {
                            return typeof(BridgeChainServiceAsync<,,,>).MakeGenericType(
                                description.InputType,
                                nextHandlerDescriptor.InputType, previousChainType,
                                description.ServiceType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            if (nextHandlerDescriptor.HasOutput)
                            {
                                return typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    nextHandlerDescriptor.InputType, nextHandlerDescriptor.OutputType, previousChainType,
                                    description.ServiceType);
                            }
                            else
                            {
                                return typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,,>).MakeGenericType(
                                    description.InputType, description.OutputType,
                                    nextHandlerDescriptor.InputType, previousChainType,
                                    description.ServiceType);
                            }
                        }
                        else
                        {
                            return typeof(BridgeChainServiceWithoutCancellationTokenAsync<,,,>).MakeGenericType(
                                description.InputType,
                                nextHandlerDescriptor.InputType, previousChainType,
                                description.ServiceType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        if (nextHandlerDescriptor.HasOutput)
                        {
                            return typeof(BridgeChainServiceSync<,,,,,>).MakeGenericType(
                                description.InputType, description.OutputType,
                                nextHandlerDescriptor.InputType, nextHandlerDescriptor.OutputType, previousChainType,
                                description.ServiceType);
                        }
                        else
                        {
                            return typeof(BridgeChainServiceSync<,,,,>).MakeGenericType(
                                description.InputType, description.OutputType,
                                nextHandlerDescriptor.InputType, previousChainType,
                                description.ServiceType);
                        }
                    }
                    else
                    {
                        return typeof(BridgeChainServiceSync<,,,>).MakeGenericType(
                            description.InputType,
                            nextHandlerDescriptor.InputType, previousChainType,
                            description.ServiceType);
                    }
                }
            }
        }
    }
}
