namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// <para>
    ///     Kinds of chains, correspond to the handlers types.
    /// </para>
    /// <para>
    ///     It may be removed in future versions.
    /// </para>
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
