using RoyalCode.PipelineFlow.Builders;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace RoyalCode.PipelineFlow
{
    /// <summary>
    /// Used to start building a <see cref="IPipelineFactory{TFor}"/>.
    /// </summary>
    public class PipelineFactory
    {
        /// <summary>
        /// The configuration for build a <see cref="IPipelineFactory{TFor}"/>.
        /// </summary>
        /// <typeparam name="TFor">The specific type of the pipeline.</typeparam>
        /// <returns>a new instance of <see cref="PipelineFactoryConfiguration{TFor}"/>.</returns>
        public static PipelineFactoryConfiguration<TFor> Configure<TFor>() => new();
    }

    /// <summary>
    /// Internal and default implementation of <see cref="IPipelineFactory{TFor}"/>.
    /// </summary>
    /// <typeparam name="TFor">The specific type of the pipeline.</typeparam>
    internal class PipelineFactory<TFor> : IPipelineFactory<TFor>
    {
        private readonly ConcurrentDictionary<Type, Type> inputOnlyChainType = new();
        private readonly ConcurrentDictionary<Tuple<Type, Type>, Type> inputOutputChainType = new();

        private readonly PipelineChainTypeBuilder pipelineChainBuilder;
        private readonly IPipelineTypeBuilder pipelineTypeBuilder;

        public PipelineFactory(
            PipelineChainTypeBuilder<TFor> pipelineChainBuilder,
            IPipelineTypeBuilder pipelineTypeBuilder)
        {
            this.pipelineChainBuilder = pipelineChainBuilder;
            this.pipelineTypeBuilder = pipelineTypeBuilder;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPipeline<TIn> Create<TIn>()
        {
            inputOnlyChainType.TryGetValue(typeof(TIn), out var chainType);
            if (chainType is null)
                chainType = inputOnlyChainType.GetOrAdd(typeof(TIn), t => pipelineChainBuilder.Build(t, null));

            return (IPipeline<TIn>)pipelineTypeBuilder.Build(chainType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPipeline<TIn, TOut> Create<TIn, TOut>()
        {
            var key = new Tuple<Type, Type>(typeof(TIn), typeof(TOut));
            inputOutputChainType.TryGetValue(key, out var chainType);
            if (chainType is null)
                chainType = inputOutputChainType.GetOrAdd(key, k => pipelineChainBuilder.Build(k.Item1, k.Item2, null));

            return (IPipeline<TIn, TOut>)pipelineTypeBuilder.Build(chainType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create(Type inputType)
        {
            inputOnlyChainType.TryGetValue(inputType, out var chainType);
            if (chainType is null)
                chainType = inputOnlyChainType.GetOrAdd(inputType, t => pipelineChainBuilder.Build(t, null));

            return pipelineTypeBuilder.Build(chainType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Create(Type inputType, Type outputType)
        {
            var key = new Tuple<Type, Type>(inputType, outputType);
            inputOutputChainType.TryGetValue(key, out var chainType);
            if (chainType is null)
                chainType = inputOutputChainType.GetOrAdd(key, k => pipelineChainBuilder.Build(k.Item1, k.Item2, null));

            return pipelineTypeBuilder.Build(chainType);
        }
    }
}
