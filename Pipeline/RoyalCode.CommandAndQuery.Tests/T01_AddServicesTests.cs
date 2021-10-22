using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace RoyalCode.CommandAndQuery.Tests
{
    public class T01_AddServicesTests
    {
        [Fact]
        public void T01_Basic_Get_ICommandQueryBus()
        {
            var services = new ServiceCollection();

            services.AddCommandQueryBus();

            var sp = services.BuildServiceProvider();

            var bus = sp.GetService<ICommandQueryBus>();
            Assert.NotNull(bus);
        }
    }
}
