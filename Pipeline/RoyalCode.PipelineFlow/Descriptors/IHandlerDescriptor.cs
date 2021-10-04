using System;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// The descriptor of a handler contains information to determine the type of chain that will be used 
    /// and provides the delegate that will be used by the chain.
    /// </summary>
    public interface IHandlerDescriptor
    {
        Type InputType { get; }

        Type OutputType { get; }

        bool HasOutput { get; }

        bool IsAsync { get; }

        bool HasToken { get; }

        Type? ServiceType { get; }

        Delegate CreateDelegate(Type inputType, Type outputType);
    }
}
