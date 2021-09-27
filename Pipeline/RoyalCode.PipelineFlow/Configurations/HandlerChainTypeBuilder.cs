using RoyalCode.PipelineFlow.Chains;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal class HandlerChainTypeBuilder : IChainTypeBuilder
    {
        public ChainKind Kind => ChainKind.Handler;

        public Type Build(DescriptionBase description, Type? previousChainType)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            if (previousChainType is not null)
                throw new ArgumentException(
                    "The input handlers can't delegate the processing to a next handler. Previous Chain Type is not allowed.",
                    nameof(previousChainType));

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
