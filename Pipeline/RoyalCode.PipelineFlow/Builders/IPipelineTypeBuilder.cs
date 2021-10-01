using System;

namespace RoyalCode.PipelineFlow.Builders
{
    public interface IPipelineTypeBuilder
    {
        public object Build(Type chainType);
    }
}
