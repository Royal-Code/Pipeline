using System;

namespace RoyalCode.PipelineFlow.Configurations
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
}
