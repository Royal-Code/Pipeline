namespace RoyalCode.PipelineFlow.Descriptors
{
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
