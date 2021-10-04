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

    internal class DecoratorSorter : IDecoratorSorter
    {
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

    /// <summary>
    /// <para>
    ///     Describes how the decorators should be ordered. Each decorator will have its own description.
    /// </para>
    /// <para>
    ///     A sorting component (<see cref="IDecoratorSorter"/>) will use these descriptors 
    ///     to determine the order in which the decorators will be executed within the pipeline.
    /// </para>
    /// </summary>
    public class SortDescriptor
    {
        /// <summary>
        /// The default descriptor for sorting, the priority is in the middle and the position equals 100.
        /// </summary>
        public static readonly SortDescriptor Default = new()
        {
            Priority = SortingPriority.InTheMiddle,
            Position = 100
        };

        /// <summary>
        /// The decorator sorting priority.
        /// </summary>
        public SortingPriority Priority { get; set; }

        /// <summary>
        /// The position value that will be used in the sort. 
        /// This value is subordinate to the priority.
        /// </summary>
        public int Position { get; set; }
    }

    /// <summary>
    /// Define the priority for sorting decorators.
    /// </summary>
    public enum SortingPriority
    {
        /// <summary>
        /// Defines that a decorator must be first among all.
        /// </summary>
        MustBeTheFirst,

        /// <summary>
        /// defines that the decorators must be positioned and ordered at the beginning.
        /// </summary>
        InTheBeginning,

        /// <summary>
        /// Defines that the decorators must be positioned and ordered in the middle.
        /// </summary>
        InTheMiddle,

        /// <summary>
        /// Defines that the decorators must be positioned and ordered at the end.
        /// </summary>
        InTheEnding,

        /// <summary>
        /// Defines that a decorator must be last among all.
        /// </summary>
        MustBeTheLast
    }
}
