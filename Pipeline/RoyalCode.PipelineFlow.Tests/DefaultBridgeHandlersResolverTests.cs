using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class DefaultBridgeHandlersResolverTests
    {
        [Fact]
        public void _01_Action_Handler()
        {
            string backValue = null;

            var resolver = DefaultHandlersResolver.BridgeHandler<int, string>((i, next) => next(i.ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<int, Action<string>>;
            Assert.NotNull(action);

            action(1, v => backValue = v);

            Assert.Equal("1", backValue);
        }
    }
}
