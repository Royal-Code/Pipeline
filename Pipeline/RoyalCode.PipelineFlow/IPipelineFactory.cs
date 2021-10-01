using RoyalCode.PipelineFlow.Builders;
using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Concurrent;

namespace RoyalCode.PipelineFlow
{
    public interface IPipelineFactory
    {
        IPipelineFactory<TFor> For<TFor>();
    }

    public interface IPipelineFactory<TFor>
    {

        IPipeline<TIn> Create<TIn>();

        IPipeline<TIn, TOut> Create<TIn, TOut>();

        object Create(Type inputType);

        object Create(Type inputType, Type outputType);
    }

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

        public IPipeline<TIn> Create<TIn>()
        {
            inputOnlyChainType.TryGetValue(typeof(TIn), out var chainType);
            if (chainType is null)
                chainType = inputOnlyChainType.GetOrAdd(typeof(TIn), t => pipelineChainBuilder.Build(t, null));

            return (IPipeline<TIn>)pipelineTypeBuilder.Build(chainType);
        }

        public IPipeline<TIn, TOut> Create<TIn, TOut>()
        {
            var key = new Tuple<Type, Type>(typeof(TIn), typeof(TOut));
            inputOutputChainType.TryGetValue(key, out var chainType);
            if (chainType is null)
                chainType = inputOutputChainType.GetOrAdd(key, k => pipelineChainBuilder.Build(k.Item1, k.Item2, null));

            return (IPipeline<TIn, TOut>)pipelineTypeBuilder.Build(chainType);
        }

        public object Create(Type inputType)
        {
            inputOnlyChainType.TryGetValue(inputType, out var chainType);
            if (chainType is null)
                chainType = inputOnlyChainType.GetOrAdd(inputType, t => pipelineChainBuilder.Build(t, null));

            return pipelineTypeBuilder.Build(chainType);
        }

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
