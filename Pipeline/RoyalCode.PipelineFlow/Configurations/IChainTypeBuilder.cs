using RoyalCode.PipelineFlow.Chains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IChainTypeBuilder
    {
        ChainKind Kind { get; }

        Type Build(DescriptionBase description, Type? previousChainType = null);
    }

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
                                return typeof(BridgeChainDelegateAsync<,,,,>)
                                    .MakeGenericType(description.InputType, description.OutputType,
                                    description.NextInputType, description.NextOutputType, previousChainType);
                            }
                            else
                            {
                                return typeof(BridgeChainDelegateAsync<,,,>)
                                    .MakeGenericType(description.InputType, description.OutputType,
                                    description.NextInputType, previousChainType);
                            }
                        }
                        else
                        {
                            return typeof(HandlerChainDelegateAsync<>)
                                .MakeGenericType(description.InputType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            return typeof(HandlerChainDelegteWithoutCancellationTokenAsync<,>)
                                .MakeGenericType(description.InputType, description.OutputType);
                        }
                        else
                        {
                            return typeof(HandlerChainDelegateWithoutCancellationTokenAsync<>)
                                .MakeGenericType(description.InputType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        return typeof(HandlerChainDelegateSync<,>)
                            .MakeGenericType(description.InputType, description.OutputType);
                    }
                    else
                    {
                        return typeof(HandlerChainDelegateSync<>)
                            .MakeGenericType(description.InputType);
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
                            return typeof(HandlerChainServiceAsync<,,>)
                                .MakeGenericType(description.InputType, description.OutputType, description.ServiceType);
                        }
                        else
                        {
                            return typeof(HandlerChainServiceAsync<,>)
                                .MakeGenericType(description.InputType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            return typeof(HandlerChainServiceWithoutCancellationTokenAsync<,,>)
                                .MakeGenericType(description.InputType, description.OutputType);
                        }
                        else
                        {
                            return typeof(HandlerChainServiceWithoutCancellationTokenAsync<,>)
                                .MakeGenericType(description.InputType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        return typeof(HandlerChainServiceSync<,,>)
                            .MakeGenericType(description.InputType, description.OutputType);
                    }
                    else
                    {
                        return typeof(HandlerChainServiceSync<,>)
                            .MakeGenericType(description.InputType);
                    }
                }
            }
        }
    }

    internal class HandlerChainTypeBuilder : IChainTypeBuilder
    {
        public ChainKind Kind => ChainKind.Handler;

        public Type Build(DescriptionBase description, Type? previousChainType)
        {
            if (description.ServiceType is null)
            {
                if (description.IsAsync)
                {
                    if (description.HasToken)
                    {
                        if (description.HasOutput)
                        {
                            return typeof(HandlerChainDelegateAsync<,>)
                                .MakeGenericType(description.InputType, description.OutputType);
                        }
                        else
                        {
                            return typeof(HandlerChainDelegateAsync<>)
                                .MakeGenericType(description.InputType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            return typeof(HandlerChainDelegteWithoutCancellationTokenAsync<,>)
                                .MakeGenericType(description.InputType, description.OutputType);
                        }
                        else
                        {
                            return typeof(HandlerChainDelegateWithoutCancellationTokenAsync<>)
                                .MakeGenericType(description.InputType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        return typeof(HandlerChainDelegateSync<,>)
                            .MakeGenericType(description.InputType, description.OutputType);
                    }
                    else
                    {
                        return typeof(HandlerChainDelegateSync<>)
                            .MakeGenericType(description.InputType);
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
                            return typeof(HandlerChainServiceAsync<,,>)
                                .MakeGenericType(description.InputType, description.OutputType, description.ServiceType);
                        }
                        else
                        {
                            return typeof(HandlerChainServiceAsync<,>)
                                .MakeGenericType(description.InputType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            return typeof(HandlerChainServiceWithoutCancellationTokenAsync<,,>)
                                .MakeGenericType(description.InputType, description.OutputType);
                        }
                        else
                        {
                            return typeof(HandlerChainServiceWithoutCancellationTokenAsync<,>)
                                .MakeGenericType(description.InputType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        return typeof(HandlerChainServiceSync<,,>)
                            .MakeGenericType(description.InputType, description.OutputType);
                    }
                    else
                    {
                        return typeof(HandlerChainServiceSync<,>)
                            .MakeGenericType(description.InputType);
                    }
                }
            }
        }
    }
}
