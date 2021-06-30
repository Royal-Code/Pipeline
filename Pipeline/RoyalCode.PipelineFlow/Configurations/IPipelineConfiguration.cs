using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineConfiguration
    {

    }

    public interface IPipelineConfiguration<TFor> : IPipelineConfiguration
    {

    }

    public interface IChainBuilder
    {
        ChainKind Kind { get; }

        Type Build(HandlerDescription handlerDescription, Type previousChainType);
        Type Build(HandlerDescription handlerDescription);
    }
}
