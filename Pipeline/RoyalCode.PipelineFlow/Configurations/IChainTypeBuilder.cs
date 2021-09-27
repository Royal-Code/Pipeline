using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IChainTypeBuilder
    {
        ChainKind Kind { get; }

        Type Build(DescriptionBase description, Type? previousChainType = null);
    }
}
