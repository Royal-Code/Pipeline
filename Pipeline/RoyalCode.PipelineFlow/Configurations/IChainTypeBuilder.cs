using RoyalCode.PipelineFlow.Chains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IChainTypeBuilder
    {
        ChainKind Kind { get; }

        Type Build(DescriptionBase description, Type? previousChainType);
    }

    public interface IChainDelegateProvider<TDelegate>
        where TDelegate : Delegate
    {
        TDelegate Delegate { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
    }

    internal class ChainDelegateProvider<TDelegate> : IChainDelegateProvider<TDelegate>
        where TDelegate : Delegate
    {
        public ChainDelegateProvider(ChainDelegateRegistry registry)
        {
            Delegate = registry.GetDelegate<TDelegate>();
        }

        public TDelegate Delegate { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
    }

    internal class ChainDelegateRegistry
    {
        private readonly ICollection<Delegate> delegates = new LinkedList<Delegate>();

        internal void AddDelegate(Delegate chainDelegate) => delegates.Add(chainDelegate);

        internal TDelegate GetDelegate<TDelegate>()
        {
            return delegates.OfType<TDelegate>().FirstOrDefault();
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
                            return typeof(HandlerChainFuncAsync<,>)
                                .MakeGenericType(description.InputType, description.OutputType);
                        }
                        else
                        {
                            return typeof(HandlerChainFuncAsync<>)
                                .MakeGenericType(description.InputType);
                        }
                    }
                    else
                    {
                        if (description.HasOutput)
                        {
                            return typeof(HandlerChainFuncWithoutCancellationTokenAsync<,>)
                                .MakeGenericType(description.InputType, description.OutputType);
                        }
                        else
                        {
                            return typeof(HandlerChainFuncWithoutCancellationTokenAsync<>)
                                .MakeGenericType(description.InputType);
                        }
                    }
                }
                else
                {
                    if (description.HasOutput)
                    {
                        return typeof(HandlerChainFuncSync<,>)
                            .MakeGenericType(description.InputType, description.OutputType);
                    }
                    else
                    {
                        return typeof(HandlerChainActionSync<>)
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

        public Type Build(DecoratorDescription decoratorDescription, Type previousChainType)
        {
            throw new NotImplementedException();
        }

        public Type Build(HandlerDescription handlerDescription, Type previousChainType)
        {
            throw new NotImplementedException();
        }

        public Type Build(HandlerDescription handlerDescription)
        {
            throw new NotImplementedException();
        }
    }
}
