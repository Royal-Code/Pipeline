using System;
using System.Runtime.CompilerServices;

namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// This interface provides the chain component delegate.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate, required for the chain component.</typeparam>
    public interface IChainDelegateProvider<TDelegate>
        where TDelegate : Delegate
    {
        /// <summary>
        /// The chain component delegate.
        /// </summary>
        TDelegate Delegate { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
    }
}
