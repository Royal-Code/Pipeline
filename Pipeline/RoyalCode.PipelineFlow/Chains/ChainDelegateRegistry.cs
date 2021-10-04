using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.Chains
{
    internal class ChainDelegateRegistry
    {
        private readonly ICollection<Delegate> delegates = new LinkedList<Delegate>();

        internal void AddDelegate(Delegate chainDelegate) => delegates.Add(chainDelegate);

        internal TDelegate? GetDelegate<TDelegate>()
        {
            return delegates.OfType<TDelegate>().FirstOrDefault();
        }
    }
}
