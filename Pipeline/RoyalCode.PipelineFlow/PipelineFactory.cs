using RoyalCode.PipelineFlow.Builders;
using RoyalCode.PipelineFlow.Chains;
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
        /// The <see cref="ChainDelegateRegistry"/> used to configure pipelines.
        /// </summary>
        public static ChainDelegateRegistry ChainDelegateRegistry { get; } = new();

        /// <summary>
        /// The configuration for build a <see cref="IPipelineFactory{TFor}"/>.
        /// </summary>
        /// <typeparam name="TFor">The specific type of the pipeline.</typeparam>
        /// <returns>a new instance of <see cref="PipelineFactoryConfiguration{TFor}"/>.</returns>
        public static PipelineFactoryConfiguration<TFor> Configure<TFor>() => new(ChainDelegateRegistry);
    }

    /// <summary>
    /// Internal and default implementation of <see cref="IPipelineFactory{TFor}"/>.
    /// </summary>
    /// <typeparam name="TFor">The specific type of the pipeline.</typeparam>
    public class PipelineFactory<TFor> : IPipelineFactory<TFor>
    {
        private static readonly ConcurrentDictionary<Type, Type> inputOnlyChainType = new();
        private static readonly ConcurrentDictionary<Type, Type> callerInputOnlyChainType = new();
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, Type> inputOutputChainType = new();
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, Type> callerInputOutputChainType = new();

        private readonly IPipelineChainTypeBuilder pipelineChainBuilder;
        private readonly IPipelineTypeBuilder pipelineTypeBuilder;

        /// <summary>
        /// Create a new pipeline factory for one specific type of the pipeline.
        /// </summary>
        /// <param name="pipelineChainBuilder"></param>
        /// <param name="pipelineTypeBuilder"></param>
        public PipelineFactory(
            IPipelineChainTypeBuilder<TFor> pipelineChainBuilder,
            IPipelineTypeBuilder pipelineTypeBuilder)
        {
            this.pipelineChainBuilder = pipelineChainBuilder;
            this.pipelineTypeBuilder = pipelineTypeBuilder;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPipeline<TIn> Create<TIn>()
        {
            inputOnlyChainType.TryGetValue(typeof(TIn), out var chainType);
            if (chainType is null)
                chainType = inputOnlyChainType.GetOrAdd(typeof(TIn), t => pipelineChainBuilder.Build(t, null));

            return (IPipeline<TIn>)pipelineTypeBuilder.Build(chainType);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPipeline<TIn, TOut> Create<TIn, TOut>()
        {
            var key = new Tuple<Type, Type>(typeof(TIn), typeof(TOut));
            inputOutputChainType.TryGetValue(key, out var chainType);
            if (chainType is null)
                chainType = inputOutputChainType.GetOrAdd(key, k => pipelineChainBuilder.Build(k.Item1, k.Item2, null));

            return (IPipeline<TIn, TOut>)pipelineTypeBuilder.Build(chainType);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPipelineCaller Create(Type inputType)
        {
            callerInputOnlyChainType.TryGetValue(inputType, out var callerType);
            if (callerType is null)
                callerType = callerInputOnlyChainType.GetOrAdd(inputType, t =>
                {
                    var chainType = pipelineChainBuilder.Build(t, null);
                    return typeof(PipelineCaller<,>).MakeGenericType(chainType, inputType);
                });

            return (IPipelineCaller)pipelineTypeBuilder.Build(callerType);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPipelineCaller<TOut> Create<TOut>(Type inputType)
        {
            var key = new Tuple<Type, Type>(inputType, typeof(TOut));
            callerInputOutputChainType.TryGetValue(key, out var callerType);
            if (callerType is null)
                callerType = callerInputOutputChainType.GetOrAdd(key, k =>
                {
                    var chainType = pipelineChainBuilder.Build(k.Item1, k.Item2, null);
                    return typeof(PipelineCaller<,,>).MakeGenericType(chainType, inputType, typeof(TOut));
                });

            return (IPipelineCaller<TOut>)pipelineTypeBuilder.Build(callerType);
        }
    }
}
