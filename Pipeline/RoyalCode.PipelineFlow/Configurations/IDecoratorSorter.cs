using RoyalCode.PipelineFlow.Descriptors;
using System.Collections.Generic;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IDecoratorSorter
    {
        IEnumerable<DecoratorDescriptor> Sort(IEnumerable<DecoratorDescriptor> descriptions);
    }
}
