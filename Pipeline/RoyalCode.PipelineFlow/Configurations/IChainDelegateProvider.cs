using System;
using System.Runtime.CompilerServices;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IChainDelegateProvider<TDelegate>
        where TDelegate : Delegate
    {
        TDelegate Delegate { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
    }
}
