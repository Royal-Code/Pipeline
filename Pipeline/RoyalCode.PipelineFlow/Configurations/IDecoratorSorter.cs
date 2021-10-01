using System.Collections.Generic;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IDecoratorSorter
    {
        IEnumerable<DecoratorDescription> Sort(IEnumerable<DecoratorDescription> descriptions);
    }
}
