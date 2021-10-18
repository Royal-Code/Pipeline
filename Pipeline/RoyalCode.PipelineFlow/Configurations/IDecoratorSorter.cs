using RoyalCode.PipelineFlow.Descriptors;
using System.Collections.Generic;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// This interface performs the sorting of the decorators in a pipeline.
    /// </summary>
    public interface IDecoratorSorter
    {
        /// <summary>
        /// It applies the decorator ordinance.
        /// </summary>
        /// <param name="descriptions">The descriptors of the decorators of a pipeline that are to be sorted.</param>
        /// <returns>A Pipeline's decorator descriptors sorted.</returns>
        IEnumerable<DecoratorDescriptor> Sort(IEnumerable<DecoratorDescriptor> descriptions);
    }
}
