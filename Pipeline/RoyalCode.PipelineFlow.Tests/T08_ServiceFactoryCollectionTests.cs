using System;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T08_ServiceFactoryCollectionTests
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

        [Fact]
        public void T04_ActivableService2_WithOptionalConstructor()
        {
            var services = new ServiceFactoryCollection();
            var provider = services.BuildServiceProvider();

            var service = provider.GetService(typeof(ActivableService2)) as ActivableService2;
            Assert.NotNull(service);
        }

        [Fact]
        public void T05_ActivableService2_ComplexService()
        {
            var services = new ServiceFactoryCollection();
            var provider = services.BuildServiceProvider();

            var service = provider.GetService(typeof(ActivableService3)) as ActivableService3;
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

    public class ActivableService2
    {
        public ActivableService2(ISimpleService? willBeNull = null) { }
    }

    public class ActivableService3
    {
        public ActivableService3(
            ActivableService service1, 
            ActivableService2 service2,
            ISimpleService? service3 = null) { }
    }
}
