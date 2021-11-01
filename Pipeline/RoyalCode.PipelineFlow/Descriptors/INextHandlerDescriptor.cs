using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// Descriptor of a next handler, like bridge handlers.
    /// </summary>
    public interface INextHandlerDescriptor
    {
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
    }
}
