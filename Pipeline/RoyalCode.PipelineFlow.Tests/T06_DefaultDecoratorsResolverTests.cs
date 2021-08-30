using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T06_DefaultDecoratorsResolverTests
    {
        [Fact]
        public void T01_Action_Decorator()
        {
            int backValue1 = 0, backValue2 = 0;

            var resolver = DefaultDecoratorsResolver.Decorate<int>((i, next) => { backValue1 = i; next(); });
            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<int, Action>;
            Assert.NotNull(action);

            action!(1, () => backValue2 = 2);

            Assert.Equal(1, backValue1);
            Assert.Equal(2, backValue2);
        }
    }
}
