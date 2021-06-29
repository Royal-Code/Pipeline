namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// Abstração para decoradores de pipeline.
    /// </summary>
    /// <typeparam name="TIn">Tipo de entrada.</typeparam>
    public abstract class DecoratorChain<TIn, TNext> : Chain<TIn>
        where TNext: Chain<TIn>
    { }

    /// <summary>
    /// Abstração para decoradores de pipeline.
    /// </summary>
    /// <typeparam name="TIn">Tipo de entrada.</typeparam>
    /// <typeparam name="TOut">Tipo de saída.</typeparam>
    public abstract class DecoratorChain<TIn, TOut, TNext> : Chain<TIn, TOut>
        where TNext : Chain<TIn, TOut>
    { }
}
