using RoyalCode.PipelineFlow.Chains;
using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Builders
{
    internal class HandlerChainTypeBuilder : IChainTypeBuilder
    {
        public ChainKind Kind => ChainKind.Handler;

        public Type Build(IHandlerDescriptor descriptor, Type? previousChainType)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            if (previousChainType is not null)
                throw new ArgumentException(
                    "The input handlers can't delegate the processing to a next handler. Previous Chain Type is not allowed.",
                    nameof(previousChainType));

            if (descriptor.ServiceType is null)
            {
                if (descriptor.IsAsync)
                {
                    if (descriptor.HasToken)
                    {
                        if (descriptor.HasOutput)
                        {
                            return typeof(HandlerChainDelegateAsync<,>)
                                .MakeGenericType(descriptor.InputType, descriptor.OutputType);
                        }
                        else
                        {
                            return typeof(HandlerChainDelegateAsync<>)
                                .MakeGenericType(descriptor.InputType);
                        }
                    }
                    else
                    {
                        if (descriptor.HasOutput)
                        {
                            return typeof(HandlerChainDelegteWithoutCancellationTokenAsync<,>)
                                .MakeGenericType(descriptor.InputType, descriptor.OutputType);
                        }
                        else
                        {
                            return typeof(HandlerChainDelegateWithoutCancellationTokenAsync<>)
                                .MakeGenericType(descriptor.InputType);
                        }
                    }
                }
                else
                {
                    if (descriptor.HasOutput)
                    {
                        return typeof(HandlerChainDelegateSync<,>)
                            .MakeGenericType(descriptor.InputType, descriptor.OutputType);
                    }
                    else
                    {
                        return typeof(HandlerChainDelegateSync<>)
                            .MakeGenericType(descriptor.InputType);
                    }
                }
            }
            else
            {
                if (descriptor.IsAsync)
                {
                    if (descriptor.HasToken)
                    {
                        if (descriptor.HasOutput)
                        {
                            return typeof(HandlerChainServiceAsync<,,>)
                                .MakeGenericType(descriptor.InputType, descriptor.OutputType, descriptor.ServiceType);
                        }
                        else
                        {
                            return typeof(HandlerChainServiceAsync<,>)
                                .MakeGenericType(descriptor.InputType, descriptor.ServiceType);
                        }
                    }
                    else
                    {
                        if (descriptor.HasOutput)
                        {
                            return typeof(HandlerChainServiceWithoutCancellationTokenAsync<,,>)
                                .MakeGenericType(descriptor.InputType, descriptor.OutputType, descriptor.ServiceType);
                        }
                        else
                        {
                            return typeof(HandlerChainServiceWithoutCancellationTokenAsync<,>)
                                .MakeGenericType(descriptor.InputType, descriptor.ServiceType);
                        }
                    }
                }
                else
                {
                    if (descriptor.HasOutput)
                    {
                        return typeof(HandlerChainServiceSync<,,>)
                            .MakeGenericType(descriptor.InputType, descriptor.OutputType, descriptor.ServiceType);
                    }
                    else
                    {
                        return typeof(HandlerChainServiceSync<,>)
                            .MakeGenericType(descriptor.InputType, descriptor.ServiceType);
                    }
                }
            }
        }
    }
}
