using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// A descriptor of a next handler, used by bridge handlers
    /// </summary>
    public class BridgeNextHandlerDescriptor
    {
        /// <summary>
        /// Create a new instance with the required information.
        /// </summary>
        /// <param name="inputType">The next handler input type.</param>
        /// <param name="outputType">The next handler output type.</param>
        /// <param name="hasOutput">If the next handler produce any result.</param>
        /// <param name="isAsync">If the next handler is async.</param>
        /// <exception cref="ArgumentNullException">
        ///     If any parameter is null.
        /// </exception>
        public BridgeNextHandlerDescriptor(Type inputType, Type outputType, bool hasOutput, bool isAsync)
        {
            InputType = inputType ?? throw new ArgumentNullException(nameof(inputType));
            OutputType = outputType ?? throw new ArgumentNullException(nameof(outputType));
            HasOutput = hasOutput;
            IsAsync = isAsync;
        }

        /// <summary>
        /// The next handler input type.
        /// </summary>
        public Type InputType { get; }

        /// <summary>
        /// The next handler output type.
        /// </summary>
        public Type OutputType { get; }

        /// <summary>
        /// If the next handler produce any result.
        /// </summary>
        public bool HasOutput { get; }

        /// <summary>
        /// If the next handler is async.
        /// </summary>
        public bool IsAsync { get; }
    }
}
