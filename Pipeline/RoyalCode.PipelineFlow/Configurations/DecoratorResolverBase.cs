using RoyalCode.PipelineFlow.Descriptors;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public abstract class DecoratorResolverBase : IDecoratorResolver
    {
        private readonly DecoratorDescriptor decoratorDescription;

        protected DecoratorResolverBase(DecoratorDescriptor decoratorDescription)
        {
            this.decoratorDescription = decoratorDescription ?? throw new ArgumentNullException(nameof(decoratorDescription));
        }

        public DecoratorDescriptor? TryResolve(Type inputType)
        {
            return decoratorDescription.Match(inputType)
                ? decoratorDescription
                : null;
        }

        public DecoratorDescriptor? TryResolve(Type inputType, Type output)
        {
            return decoratorDescription.Match(inputType, output)
                ? decoratorDescription
                : null;
        }
    }
}
