using RoyalCode.PipelineFlow.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <inheritdoc/>
    internal class DecoratorSorter : IDecoratorSorter
    {
        /// <inheritdoc/>
        public IEnumerable<DecoratorDescriptor> Sort(IEnumerable<DecoratorDescriptor> descriptions)
        {
            var groups = descriptions.GroupBy(d => d.SortDescriptor.Priority)
                .OrderBy(g => g.Key);

            foreach (var group in groups)
            {
                var items = group.ToList();
                switch (group.Key)
                {
                    case SortingPriority.MustBeTheFirst:

                        if (items.Count > 1)
                            throw new InvalidOperationException("TODO: create an exception for this case");

                        yield return items.First();

                        break;
                    case SortingPriority.InTheBeginning:
                    case SortingPriority.InTheMiddle:
                    case SortingPriority.InTheEnding:

                        foreach (var item in items)
                        {
                            yield return item;
                        }

                        break;
                    case SortingPriority.MustBeTheLast:

                        if (items.Count > 1)
                            throw new InvalidOperationException("TODO: create an exception for this case");

                        yield return items.First();

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
