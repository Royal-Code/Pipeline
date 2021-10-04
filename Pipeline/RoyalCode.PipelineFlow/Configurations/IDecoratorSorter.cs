using RoyalCode.PipelineFlow.Descriptors;
using System.Collections.Generic;

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
            throw new System.NotImplementedException();
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
