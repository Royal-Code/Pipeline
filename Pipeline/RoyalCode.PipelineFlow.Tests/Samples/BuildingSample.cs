using RoyalCode.PipelineFlow.Configurations;
using RoyalCode.PipelineFlow.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Tests.Samples
{

    public class BuildingSample
    {
        public void Buiding(IPipelineBuilder builder)
        {
            IHandlerResolver resolver = DefaultHandlersResolver
                .HandleAsync<BuildingSample>(sample => Task.FromResult(sample));

            builder.AddHandlerResolver(resolver);

            builder.Configure<BuildingSample>()
                .Handle(sample => Task.FromResult(sample))
                .Decorate((sample, next) => { next(); });

            builder.Configure<BuildingSample>()
                .Handle(sample => { })
                .WithService<IBuildingSampleService>()
                .Handle((service, sample) => { })
                .WithService<IBuildingSampleService>()
                .HandleAsync((service, sample, token) => Task.FromResult(sample));
        }

        public void Buiding(IPipelineBuilder<BuildingSample> builder)
        {

        }

        public void Buiding(IPipelineBuilder<BuildingSample, object> builder)
        {

        }
    }

    internal interface IBuildingSampleService { }
}
