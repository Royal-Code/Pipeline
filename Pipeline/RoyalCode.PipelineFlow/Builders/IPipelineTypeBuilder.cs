using System;
using RoyalCode.PipelineFlow.Chains;

namespace RoyalCode.PipelineFlow.Builders
{
    /// <summary>
    /// <para>
    ///     This inteface acts as a service provider for creating the <see cref="IPipeline{TIn}"/> 
    ///     and <see cref="IPipeline{TIn, TOut}"/> objects.
    /// </para>
    /// </summary>
    public interface IPipelineTypeBuilder
    {
        /// <summary>
        /// <para>
        ///     Creates a <see cref="IPipeline{TIn}"/> or <see cref="IPipeline{TIn, TOut}"/> 
        ///     depending on the type specified.
        /// </para>
        /// <para>
        ///     The type <see cref="Chain{TIn}"/> implements <see cref="IPipeline{TIn}"/> and 
        ///     the type <see cref="Chain{TIn, TOut}"/> implements <see cref="IPipeline{TIn, TOut}"/>.
        /// </para>
        /// <para>
        ///     The type received by parameter will implement one of the base chain types.
        /// </para>
        /// </summary>
        /// <param name="chainType">
        ///     Chain type, which will inherit either <see cref="Chain{TIn}"/> or <see cref="Chain{TIn, TOut}"/>.
        /// </param>
        /// <returns>
        ///     A new instance of the <paramref name="chainType"/>.
        /// </returns>
        public object Build(Type chainType);
    }
}
