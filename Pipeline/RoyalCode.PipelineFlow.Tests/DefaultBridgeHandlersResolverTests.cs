using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class DefaultBridgeHandlersResolverTests
    {
        [Fact]
        public void _01_Action_Bridge()
        {
            string? backValue = null;

            var resolver = DefaultHandlersResolver.BridgeHandler<int, string>((i, next) => next(i.ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<int, Action<string>>;
            Assert.NotNull(action);

            action!(1, v => backValue = v);

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void _02_Func_Bridge_Async()
        {
            string? backValue = null;

            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<int, string>((i, next) => next(i.ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string, Task>, Task>;
            Assert.NotNull(function);

            function!(1, v => { backValue = v; return Task.CompletedTask; });

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void _03_Func_Bridge_Async_WithToken()
        {
            string? backValue = null;

            var resolver = DefaultHandlersResolver.BridgeHandlerAsync<int, string>((i, next, token) => next(i.ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<int, Func<string, Task>, CancellationToken, Task>;
            Assert.NotNull(function);

            function!(1, v => { backValue = v; return Task.CompletedTask; }, default);

            Assert.Equal("1", backValue);
        }

        [Fact]
        public void _04_Service_Action_Bridge()
        {
            BackValueService backValue = new();

            var resolver = DefaultHandlersResolver.BridgeHandler<BackValueService, int, string>((s, i, next) => next(i.ToString()));

            var description = resolver.TryResolve(typeof(int));
            Assert.NotNull(description);

            var @delegate = description!.HandlerDelegateProvider(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<BackValueService, int, Action<string>>;
            Assert.NotNull(action);

            action!(backValue, 1, v => backValue.Value = v);

            Assert.Equal("1", backValue.Value);
        }



        private class BackValueService
        {
            public string? Value { get; set; }

            public BackValueService(string? value = null)
            {
                Value = value;
            }
        }
    }
}
