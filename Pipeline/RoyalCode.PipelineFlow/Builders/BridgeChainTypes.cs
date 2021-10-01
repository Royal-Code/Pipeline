using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.Builders
{
    /// <summary>
    /// Contém a sequencia dos tipos de input para validação de loop quando utilizado brigde handlers.
    /// </summary>
    public class BridgeChainTypes
    {
        private readonly Queue<Type> inputOnlyTypes = new();
        private readonly Queue<Tuple<Type, Type>> inputOutputTypes = new();

        public BridgeChainTypes(Type inputType)
        {
            if (inputType is null)
                throw new ArgumentNullException(nameof(inputType));

            inputOnlyTypes.Enqueue(inputType);
        }

        public BridgeChainTypes(Type inputType, Type outputType)
        {
            if (inputType is null)
                throw new ArgumentNullException(nameof(inputType));
            if (outputType is null)
                throw new ArgumentNullException(nameof(outputType));

            inputOutputTypes.Enqueue(new Tuple<Type, Type>(inputType, outputType));
        }

        public void Enqueue(Type nextInputType)
        {
            if (nextInputType is null)
                throw new ArgumentNullException(nameof(nextInputType));

            if (inputOnlyTypes.Contains(nextInputType))
                throw new InvalidOperationException(
                    $"A loop was found for the input type of a bridge handler of a pipeline flow. " +
                    $"The input type '{nextInputType.FullName}' was found twice. " +
                    $"The sequence of input types are: '{string.Join("' -> '", inputOnlyTypes.Select(t => t.Name))}'.");

            inputOnlyTypes.Enqueue(nextInputType);
        }

        public void Enqueue(Type nextInputType, Type outputType)
        {
            if (nextInputType is null)
                throw new ArgumentNullException(nameof(nextInputType));
            if (outputType is null)
                throw new ArgumentNullException(nameof(outputType));

            var key = new Tuple<Type, Type>(nextInputType, outputType);

            if (inputOutputTypes.Contains(key))
                throw new InvalidOperationException(
                    $"A loop was found for the input and output types of a bridge handler of a pipeline flow. " +
                    $"The input type '{nextInputType.FullName}' and output type {outputType.FullName} was found twice. " +
                    $"The sequence of types are: '{string.Join("' -> '", inputOutputTypes.Select(t => $"({t.Item1.Name}, {t.Item2.Name})"))}'.");

            inputOutputTypes.Enqueue(key);
        }
    }
}
