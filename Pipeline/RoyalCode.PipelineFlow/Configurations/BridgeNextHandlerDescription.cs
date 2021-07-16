using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public class BridgeNextHandlerDescription
    {
        public BridgeNextHandlerDescription(Type inputType, Type outputType, bool hasOutput, bool isAsync)
        {
            InputType = inputType ?? throw new ArgumentNullException(nameof(inputType));
            OutputType = outputType ?? throw new ArgumentNullException(nameof(outputType));
            HasOutput = hasOutput;
            IsAsync = isAsync;
        }

        public Type InputType { get; }

        public Type OutputType { get; }

        public bool HasOutput { get; }

        public bool IsAsync { get; }
    }
}
