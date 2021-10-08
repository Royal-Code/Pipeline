using RoyalCode.PipelineFlow.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IDecoratorSorter
    {
        IEnumerable<DecoratorDescriptor> Sort(IEnumerable<DecoratorDescriptor> descriptions);
    }
}
