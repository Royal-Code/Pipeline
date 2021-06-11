using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineFactory
    {
        IPipelineFactory<TFor> For<TFor>();
    }

    public interface IPipelineFactory<TFor>
    {

    }
}
