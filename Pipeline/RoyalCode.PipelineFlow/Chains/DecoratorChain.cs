namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// Abstraction for pipeline decorators.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    public abstract class DecoratorChain<TIn, TNext> : Chain<TIn>
        where TNext: Chain<TIn>
    { }

    /// <summary>
    /// Abstraction for pipeline decorators.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output/result type.</typeparam>
    public abstract class DecoratorChain<TIn, TOut, TNext> : Chain<TIn, TOut>
        where TNext : Chain<TIn, TOut>
    { }
}
