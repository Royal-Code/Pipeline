using RoyalCode.PipelineFlow.Extensions;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// Analyze and store information about the return type of a method, which will be the output of the pipeline.
    /// </summary>
    public class OutputDescriptor
    {
        /// <summary>
        /// Create a new instance of the descriptor for the handler method.
        /// </summary>
        /// <param name="method">The handler method.</param>
        public OutputDescriptor(MethodInfo method)
        {
            var outputType = method.ReturnType;
            bool hasOutput = true;
            bool isAsync = false;
            bool isVoid = false;

            if (outputType == typeof(void))
            {
                isVoid = true;
                hasOutput = false;
            }
            else if (outputType.Implements(typeof(Task)))
            {
                isAsync = true;

                // check if a Task<>, with result.
                if (outputType.IsGenericType)
                {
                    // get the result type.
                    outputType = outputType.GetGenericArguments()[0];
                }
                else
                {
                    // Task without result is like void.
                    hasOutput = false;
                }
            }

            OutputType = outputType;
            HasOutput = hasOutput;
            IsAsync = isAsync;
            IsVoid = isVoid;
        }

        /// <summary>
        /// <para>
        ///     The output type of handler, the pipeline result.
        /// </para>
        /// <para>
        ///     The output type may be a generic type.
        /// </para>
        /// </summary>
        public Type OutputType { get; }

        /// <summary>
        /// <para>
        ///     If the handler of the request (input type) will produce a result.
        /// </para>
        /// <para>
        ///     If return type of the method are 'void' or 'Task' the value will be false, otherwise true.
        /// </para>
        /// </summary>
        public bool HasOutput { get; }

        /// <summary>
        /// If the handler is async.
        /// </summary>
        public bool IsAsync { get; }

        /// <summary>
        /// If the output type is void.
        /// </summary>
        public bool IsVoid { get; }
    }
}
