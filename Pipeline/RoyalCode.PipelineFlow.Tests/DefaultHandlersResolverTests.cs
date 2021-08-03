using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class DefaultHandlersResolverTests
    {

        [Fact]
        public void _01_Action_Handler()
        {
            int backValue = 0;

            var resolver = DefaultHandlersResolver.Handle<int>(i => backValue = i);
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<int>;
            Assert.NotNull(action);

            action(1);

            Assert.Equal(1, backValue);
        }
    }
}
