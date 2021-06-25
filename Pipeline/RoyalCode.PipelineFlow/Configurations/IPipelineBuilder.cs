using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineBuilder
    {

    }

    public interface IPipelineBuilder<TIn> : IPipelineBuilder
    {
    }

    public interface IPipelineBuilder<TIn, TOut> : IPipelineBuilder
    {
    }
}
