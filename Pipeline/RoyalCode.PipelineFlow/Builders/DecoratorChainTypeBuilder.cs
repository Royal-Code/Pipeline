using RoyalCode.PipelineFlow.Chains;
using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Builders
{
    /// <summary>
    /// <para>
    ///     This implementation of <see cref="IChainTypeBuilder"/> handles the creation of chain types for decoratros handler descriptors.
    /// </para>
    /// </summary>
    internal class DecoratorChainTypeBuilder : IChainTypeBuilder
    {
        /// <inheritdoc/>
        public ChainKind Kind => ChainKind.Decorator;

        /// <inheritdoc/>
        public Type Build(IHandlerDescriptor descriptor, Type? previousChainType)
        {
            if (descriptor is null)
                throw new ArgumentNullException(nameof(descriptor));

            if (previousChainType is null)
                throw new ArgumentNullException(nameof(previousChainType));

            if (descriptor is not DecoratorDescriptor description)
                throw new InvalidOperationException(
                    $"{nameof(DecoratorChainTypeBuilder)} only accepts {nameof(DecoratorDescriptor)}" +
                    $" and the current instance is type of {descriptor.GetType().Name}");

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
