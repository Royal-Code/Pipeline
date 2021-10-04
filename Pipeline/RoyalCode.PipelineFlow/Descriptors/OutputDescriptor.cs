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

        public Type OutputType { get; }

        public bool HasOutput { get; }

        public bool IsAsync { get; }

        public bool IsVoid { get; }
    }
}
