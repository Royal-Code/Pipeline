using System;

namespace RoyalCode.PipelineFlow.Builders
{
    /// <summary>
    /// <para>
    ///     Internal implementation of <see cref="IPipelineTypeBuilder"/>.
    /// </para>
    /// <para>
    ///     By default, the <see cref="IPipeline{TIn}"/> e <see cref="IPipeline{TIn, TOut}"/>
    ///     are created as a service.
    /// </para>
    /// </summary>
    public class PipelineTypeBuilder : IPipelineTypeBuilder
    {
        private readonly IServiceProvider provider;

        /// <summary>
        /// Create a new instance that depends of the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="provider">Used do build the chain type.</param>
        public PipelineTypeBuilder(IServiceProvider provider)
        {
            this.provider = provider;
        }

        /// <inheritdoc/>
        public object Build(Type chainType) => provider.GetService(chainType)
            ?? throw new ArgumentException($"Invalid chain type. The type {chainType.FullName} is not a service and can't be builded.");
    }
}
