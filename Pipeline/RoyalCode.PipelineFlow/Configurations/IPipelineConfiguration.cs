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

    }
}
