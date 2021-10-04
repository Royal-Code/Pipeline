using RoyalCode.PipelineFlow.Resolvers;
using System;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IPipelineBuilder
    {
        void AddHandlerResolver(IHandlerResolver resolver);

        void AddDecoratorResolver(IDecoratorResolver resolver);

        IPipelineBuilder<TIn> Configure<TIn>();

        IPipelineBuilder<TIn, TOut> Configure<TIn, TOut>();
    }

    public interface IPipelineBuilder<TIn> : IPipelineBuilder
    {
        IPipelineBuilderWithService<TService, TIn> WithService<TService>();
    }

    public interface IPipelineBuilder<TIn, TOut> : IPipelineBuilder
    {
        IPipelineBuilderWithService<TService, TIn, TOut> WithService<TService>();
    }

    public interface IPipelineBuilderWithService<TService> : IPipelineBuilder { }

    public interface IPipelineBuilderWithService<TService, TIn> : IPipelineBuilderWithService<TService> { }

    public interface IPipelineBuilderWithService<TService, TIn, TOut> : IPipelineBuilderWithService<TService> { }

    public class DeafultPipelineBuilder : IPipelineBuilder
    {
        private readonly IPipelineConfiguration configuration;

        public DeafultPipelineBuilder(IPipelineConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void AddHandlerResolver(IHandlerResolver resolver)
        {
            configuration.Handlers.Add(resolver);
        }

        public void AddDecoratorResolver(IDecoratorResolver resolver)
        {
            configuration.Decorators.Add(resolver);
        }

        public IPipelineBuilder<TIn> Configure<TIn>() => new DefaultPipelineBuilder<TIn>(this);

        public IPipelineBuilder<TIn, TOut> Configure<TIn, TOut>() => new DefaultPipelineBuilder<TIn, TOut>(this);
    }

    public abstract class PipelineBuilderBase : IPipelineBuilder
    {
        protected readonly IPipelineBuilder pipelineBuilder;

        internal protected PipelineBuilderBase(IPipelineBuilder pipelineBuilder)
        {
            this.pipelineBuilder = pipelineBuilder ?? throw new ArgumentNullException(nameof(pipelineBuilder));
        }

        public void AddHandlerResolver(IHandlerResolver resolver) => pipelineBuilder.AddHandlerResolver(resolver);

        public void AddDecoratorResolver(IDecoratorResolver resolver) => pipelineBuilder.AddDecoratorResolver(resolver);

        public IPipelineBuilder<TInput> Configure<TInput>() => pipelineBuilder.Configure<TInput>();

        public IPipelineBuilder<TInput, TOut> Configure<TInput, TOut>() => pipelineBuilder.Configure<TInput, TOut>();
    }

    public class DefaultPipelineBuilderWithService<TService> : PipelineBuilderBase, IPipelineBuilderWithService<TService>
    {
        public DefaultPipelineBuilderWithService(IPipelineBuilder builder)
            : base(builder)
        { }
    }

    public class DefaultPipelineBuilder<TIn> : PipelineBuilderBase, IPipelineBuilder<TIn>
    {
        public DefaultPipelineBuilder(IPipelineBuilder pipelineBuilder)
            : base(pipelineBuilder)
        { }

        public IPipelineBuilderWithService<TService, TIn> WithService<TService>() => new DefaultPipelineBuilderWithService<TService, TIn>(pipelineBuilder);
    }

    public class DefaultPipelineBuilderWithService<TService, TIn> : DefaultPipelineBuilder<TIn>, IPipelineBuilderWithService<TService, TIn>
    {
        public DefaultPipelineBuilderWithService(IPipelineBuilder pipelineBuilder) : base(pipelineBuilder) { }
    }

    public class DefaultPipelineBuilder<TIn, TOut> : PipelineBuilderBase, IPipelineBuilder<TIn, TOut>
    {
        public DefaultPipelineBuilder(IPipelineBuilder pipelineBuilder) : base(pipelineBuilder) { }

        public IPipelineBuilderWithService<TService, TIn, TOut> WithService<TService>() => new DefaultPipelineBuilderWithService<TService, TIn, TOut>(pipelineBuilder);
    }

    public class DefaultPipelineBuilderWithService<TService, TIn, TOut> : DefaultPipelineBuilder<TIn, TOut>, IPipelineBuilderWithService<TService, TIn, TOut>
    {
        public DefaultPipelineBuilderWithService(IPipelineBuilder pipelineBuilder) : base(pipelineBuilder) { }
    }
}
