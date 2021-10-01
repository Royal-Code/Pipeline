using System;
using System.Runtime.CompilerServices;

namespace RoyalCode.PipelineFlow.Chains
{
    public interface IChainDelegateProvider<TDelegate>
        where TDelegate : Delegate
    {
        TDelegate Delegate { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
    }
}
