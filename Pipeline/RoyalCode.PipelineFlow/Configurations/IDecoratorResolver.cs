using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IDecoratorResolver
    {
        DecoratorDescription? TryResolve(Type inputType);

        DecoratorDescription? TryResolve(Type inputType, Type output);
    }

    public abstract class DecoratorResolverBase : IDecoratorResolver
    {
        private readonly DecoratorDescription decoratorDescription;

        protected DecoratorResolverBase(DecoratorDescription decoratorDescription)
        {
            this.decoratorDescription = decoratorDescription ?? throw new ArgumentNullException(nameof(decoratorDescription));
        }

        public DecoratorDescription? TryResolve(Type inputType)
        {
            return decoratorDescription.Match(inputType)
                ? decoratorDescription
                : null;
        }

        public DecoratorDescription? TryResolve(Type inputType, Type output)
        {
            return decoratorDescription.Match(inputType, output)
                ? decoratorDescription
                : null;
        }
    }

    public class DelegateDecoratorResolver : DecoratorResolverBase
    {
        public DelegateDecoratorResolver(Delegate decoratorHandler)
            : base(decoratorHandler.GetDecoratorDescription())
        { }
    }
}
