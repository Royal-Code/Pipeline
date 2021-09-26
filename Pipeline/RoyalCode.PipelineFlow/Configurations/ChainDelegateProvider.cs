using System;
using System.Runtime.CompilerServices;

namespace RoyalCode.PipelineFlow.Configurations
{
    internal class ChainDelegateProvider<TDelegate> : IChainDelegateProvider<TDelegate>
        where TDelegate : Delegate
    {
        public ChainDelegateProvider(ChainDelegateRegistry registry)
        {
            Delegate = registry.GetDelegate<TDelegate>();
        }

        public TDelegate Delegate { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
    }
}
