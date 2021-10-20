using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCode.PipelineFlow.Builders
{
    /// <summary>
    /// Contains the sequence of input types for loop validation when using brigde handlers.
    /// </summary>
    public class BridgeChainTypes
    {
        private readonly Queue<Type> inputOnlyTypes = new();
        private readonly Queue<Tuple<Type, Type>> inputOutputTypes = new();

        /// <summary>
        /// Creates a new instance with the first input type of the pipeline.
        /// </summary>
        /// <param name="inputType">The first input type of the pipeline.</param>
        /// <exception cref="ArgumentNullException">
        ///     Case <paramref name="inputType"/> is null.
        /// </exception>
        public BridgeChainTypes(Type inputType)
        {
            if (inputType is null)
                throw new ArgumentNullException(nameof(inputType));

            inputOnlyTypes.Enqueue(inputType);
        }

        /// <summary>
        /// Creates a new instance with the first input and output types of the pipeline.
        /// </summary>
        /// <param name="inputType">The first input type of the pipeline.</param>
        /// <param name="outputType">The first output type of the pipeline.</param>
        /// <exception cref="ArgumentNullException">
        ///     Case <paramref name="inputType"/> is null,
        ///     or case <paramref name="outputType"/> is null.
        /// </exception>
        public BridgeChainTypes(Type inputType, Type outputType)
        {
            if (inputType is null)
                throw new ArgumentNullException(nameof(inputType));
            if (outputType is null)
                throw new ArgumentNullException(nameof(outputType));

            inputOutputTypes.Enqueue(new Tuple<Type, Type>(inputType, outputType));
        }

        /// <summary>
        /// Enqueue the next input type of the pipeline.
        /// </summary>
        /// <param name="nextInputType">The next input type of the pipeline.</param>
        /// <exception cref="ArgumentNullException">
        ///     Case <paramref name="nextInputType"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     If a dependency loop occurs, where the input type has already been added before.
        /// </exception>
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

        /// <summary>
        /// Enqueue the next input and output types of the pipeline.
        /// </summary>
        /// <param name="nextInputType">The next input type of the pipeline.</param>
        /// <param name="outputType">The next output type of the pipeline.</param>
        /// <exception cref="ArgumentNullException">
        ///     Case <paramref name="nextInputType"/> is null,
        ///     or case <paramref name="outputType"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     If a dependency loop occurs, where the input and output types has already been added before.
        /// </exception>
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
