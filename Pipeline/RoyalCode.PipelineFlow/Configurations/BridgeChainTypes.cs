using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// Contém a sequencia dos tipos de input para validação de loop quando utilizado brigde handlers.
    /// </summary>
    public class BridgeChainTypes
    {
        private readonly Queue<Type> types;

        public BridgeChainTypes(Type inputType)
        {
            if (inputType is null)
                throw new ArgumentNullException(nameof(inputType));

            types = new Queue<Type>();
            types.Enqueue(inputType);
        }

        public void Enqueue(Type nextInputType)
        {
            if (nextInputType is null)
                throw new ArgumentNullException(nameof(nextInputType));

            if (types.Contains(nextInputType))
                throw new InvalidOperationException(
                    $"A loop was found for the input type of a bridge handler of a pipeline flow. " +
                    $"The input type '{nextInputType.FullName}' was found twice. " +
                    $"The sequence of input types is: '{string.Join("' -> '", types.Select(t => t.Name))}'.");

            types.Enqueue(nextInputType);
        }
    }
}
