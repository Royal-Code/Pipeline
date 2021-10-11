using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T09_ServiceFactoryCollectionTests
    {

        [Fact]
        public void T01_ServiceNotConfigurated()
        {
            var services = new ServiceFactoryCollection();
            var provider = services.BuildServiceProvider();

            Assert.Throws<ArgumentException>(() =>
            {
                var service = provider.GetService(typeof(IServiceNotConfigurated));
            });
        }

        [Fact]
        public void T02_SimpleService()
        {
            var services = new ServiceFactoryCollection();
            services.AddServiceFactory<ISimpleService>((type, provider) => new SimpleService(1));

            var provider = services.BuildServiceProvider();

            var service = (ISimpleService)provider.GetService(typeof(ISimpleService))!;

            Assert.Equal(1, service.Value);
        }

        [Fact]
        public void T03_ActivableService()
        {
            var services = new ServiceFactoryCollection();
            var provider = services.BuildServiceProvider();

            var service = provider.GetService(typeof(ActivableService)) as ActivableService;
            Assert.NotNull(service);
        }


    }


    public interface IServiceNotConfigurated { }

    public interface ISimpleService
    {
        int Value { get; }
    }

    public class SimpleService : ISimpleService
    {
        public int Value { get; }

        public SimpleService(int value)
        {
            Value = value;
        }
    }

    public class ActivableService
    {
        public ActivableService() { }
    }
}
