namespace RoyalCode.PipelineFlow.Descriptors
{
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
}
