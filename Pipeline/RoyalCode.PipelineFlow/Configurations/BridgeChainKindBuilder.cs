using RoyalCode.PipelineFlow.Chains;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal class BridgeChainKindBuilder : IChainTypeBuilder
    {
        public ChainKind Kind => ChainKind.Bridge;

        public Type Build(DescriptionBase baseDescription, Type? previousChainType)
        {
            if (baseDescription is null)
                throw new ArgumentNullException(nameof(baseDescription));

            if (previousChainType is null)
                throw new ArgumentNullException(nameof(previousChainType));

            if (baseDescription is not BridgeDescription description)
                throw new InvalidOperationException(
                    $"{nameof(BridgeChainKindBuilder)} only accepts {nameof(BridgeDescription)}" +
                    $" and the current instance is type of {baseDescription.GetType().Name}");

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
