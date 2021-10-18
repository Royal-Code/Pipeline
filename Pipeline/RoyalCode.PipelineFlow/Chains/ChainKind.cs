namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// Kinds of chains, correspond to the handlers types.
    /// </summary>
    public enum ChainKind
    {
        /// <summary>
        /// A Processing handler.
        /// </summary>
        Handler,

        /// <summary>
        /// A bridge handler.
        /// </summary>
        Bridge,

        /// <summary>
        /// A decorator handler.
        /// </summary>
        Decorator
    }
}
