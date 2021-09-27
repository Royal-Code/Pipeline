using RoyalCode.PipelineFlow.Chains;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal class DecoratorChainTypeBuilder : IChainTypeBuilder
    {
        public ChainKind Kind => ChainKind.Decorator;

        public Type Build(DescriptionBase baseDescription, Type? previousChainType)
        {
            if (baseDescription is null)
                throw new ArgumentNullException(nameof(baseDescription));

            if (previousChainType is null)
                throw new ArgumentNullException(nameof(previousChainType));

            if (baseDescription is not DecoratorDescription description)
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
                            return typeof(DecoratorChainDelegateAsync<,,>)
                                .MakeGenericType(description.InputType, description.OutputType, previousChainType);
                        }
                        else
                        {
                            return typeof(DecoratorChainDelegateAsync<,>)
                                .MakeGenericType(description.InputType, previousChainType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            return typeof(DecoratorChainDelegateWithoutCancellationTokenAsync<,,>)
                                .MakeGenericType(description.InputType, description.OutputType, previousChainType);
                        }
                        else
                        {
                            return typeof(DecoratorChainDelegateWithoutCancellationTokenAsync<,>)
                                .MakeGenericType(description.InputType, previousChainType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        return typeof(DecoratorChainDelegateSync<,,>)
                            .MakeGenericType(description.InputType, description.OutputType, previousChainType);
                    }
                    else
                    {
                        return typeof(DecoratorChainDelegateSync<,>)
                            .MakeGenericType(description.InputType, previousChainType);
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
                            return typeof(DecoratorChainServiceAsync<,,,>).MakeGenericType(
                                description.InputType, description.OutputType, 
                                previousChainType, description.ServiceType);
                        }
                        else
                        {
                            return typeof(DecoratorChainServiceAsync<,,>)
                                .MakeGenericType(description.InputType, previousChainType, description.ServiceType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            return typeof(DecoratorChainServiceWithoutCancellationTokenAsync<,,,>).MakeGenericType(
                                description.InputType, description.OutputType,
                                previousChainType, description.ServiceType);
                        }
                        else
                        {
                            return typeof(DecoratorChainServiceWithoutCancellationTokenAsync<,,>)
                                .MakeGenericType(description.InputType, previousChainType, description.ServiceType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        return typeof(DecoratorChainServiceSync<,,,>).MakeGenericType(
                            description.InputType, description.OutputType,
                            previousChainType, description.ServiceType);
                    }
                    else
                    {
                        return typeof(DecoratorChainServiceSync<,,>)
                            .MakeGenericType(description.InputType, previousChainType, description.ServiceType);
                    }
                }
            }
        }
    }
}
