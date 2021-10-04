using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// Chains components use delegates to enforce the execution of a handler.
    /// These delegates are created from descriptors and then must be stored/registered here 
    /// to be used by the chain components.
    /// </summary>
    internal class ChainDelegateRegistry
    {
        private readonly ICollection<Delegate> delegates = new LinkedList<Delegate>();

        /// <summary>
        /// Register a delegate.
        /// </summary>
        /// <param name="chainDelegate">The delegate for some chain component.</param>
        internal void AddDelegate(Delegate chainDelegate) => delegates.Add(chainDelegate);

        /// <summary>
        /// Retrieve the chain component delegate.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
        /// <returns>The delegate or null case the delegate was not registred before.</returns>
        internal TDelegate? GetDelegate<TDelegate>()
        {
            return delegates.OfType<TDelegate>().FirstOrDefault();
        }
    }
}
