using System;
using System.Runtime.CompilerServices;

namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// <para>
    ///     Default implementation for <see cref="IChainDelegateProvider{TDelegate}"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TDelegate">
    ///     The type of the delegate, required for the chain component.
    /// </typeparam>
    internal class ChainDelegateProvider<TDelegate> : IChainDelegateProvider<TDelegate>
        where TDelegate : Delegate
    {
        /// <summary>
        /// Create a new <see cref="IChainDelegateProvider{TDelegate}"/>.
        /// </summary>
        /// <param name="registry"></param>
        public ChainDelegateProvider(ChainDelegateRegistry registry)
        {
            if (registry is null)
                throw new ArgumentNullException(nameof(registry));

            Delegate = registry.GetDelegate<TDelegate>() 
                ?? throw new InvalidOperationException(
                    $"The delegate type '{typeof(TDelegate).Name}' was not registrated on {nameof(ChainDelegateRegistry)}");
        }

        /// <inheritdoc/>
        public TDelegate Delegate { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
    }
}
