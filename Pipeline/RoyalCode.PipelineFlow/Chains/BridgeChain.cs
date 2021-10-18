namespace RoyalCode.PipelineFlow.Chains
{
    /// <inheritdoc/>
    public abstract class BridgeChain<TIn, TNextIn, TNextChain> : Chain<TIn> 
        where TNextChain : Chain<TNextIn>
    { }

    /// <inheritdoc/>
    public abstract class BridgeChain<TIn, TOut, TNextIn, TNextChain> : Chain<TIn, TOut>
        where TNextChain : Chain<TNextIn, TOut>
    { }

    /// <inheritdoc/>
    public abstract class BridgeChain<TIn, TOut, TNextIn, TNextOut, TNextChain> : Chain<TIn, TOut>
        where TNextChain : Chain<TNextIn, TNextOut>
    { }
}
