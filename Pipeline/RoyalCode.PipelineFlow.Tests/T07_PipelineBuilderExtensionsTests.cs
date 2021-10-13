using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T07_PipelineBuilderExtensionsTests
    {
        [Fact]
        public void T01_Handle_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.Handle(i => { });

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T02_ServiceHandle_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderTestService>()
                .Handle((s, i) => { });

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T03_HandleAsync_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.HandleAsync((i, t) => Task.CompletedTask);

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T04_HandleAsyncWithoutCancellationToken_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.HandleAsync(i => Task.CompletedTask);

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T05_ServiceHandleAsync_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderTestService>()
                .HandleAsync((s, i, t) => Task.CompletedTask);

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T06_ServiceHandleAsyncWithoutCancellationToken_In()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.WithService<PipelineBuilderTestService>()
                .HandleAsync((s, i) => Task.CompletedTask);

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T07_Handle_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.Handle(i => i.ToString());

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T08_ServiceHandle_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderTestService>()
                .Handle((s, i) => i.ToString());

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T09_HandleAsync_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.HandleAsync((i, t) => Task.FromResult(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T10_HandleAsyncWithoutCancellationToken_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.HandleAsync(i => Task.FromResult(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T11_ServiceHandleAsync_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderTestService>()
                .HandleAsync((s, i, t) => Task.FromResult(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T12_ServiceHandleAsyncWithoutCancellationToken_InOut()
        {
            var builder = TestPipelineBuilder.Create<int, string>();

            builder.WithService<PipelineBuilderTestService>()
                .HandleAsync((s, i) => Task.FromResult(i.ToString()));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T13_MethodHandle_SingleMethod()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.Handle(PipelineBuilderTestService.GetHandlerMethod());

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
        }

        [Fact]
        public void T14_MethodHandle_ArrayOfMethods()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddHandlersMethods(PipelineBuilderTestService.GetHandlerMethods());

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
            description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }

        [Fact]
        public void T15_MethodHandle_ByAttribute()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddHandlersMethodsDefined<PipelineBuilderTestService, TestPipelineBuilderAttribute>();

            var description = builder.Handlers.GetDescription(typeof(int));
            Assert.NotNull(description);
            description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.Null(description);
        }

        [Fact]
        public void T16_MethodHandle_ByName()
        {
            var builder = TestPipelineBuilder.Create<int>();

            builder.AddHandlerMethodDefined<PipelineBuilderTestService>(nameof(PipelineBuilderTestService.HandleAsync));

            var description = builder.Handlers.GetDescription(typeof(int), typeof(string));
            Assert.NotNull(description);
        }
    }

    internal class PipelineBuilderRegistry : IPipelineConfiguration
    {
        public HandlerRegistry Handlers { get; } = new HandlerRegistry();

        public DecoratorRegistry Decorators { get; } = new DecoratorRegistry();
    }

    internal class TestPipelineBuilder : DefaultPipelineBuilder
    {
        public TestPipelineBuilder(PipelineBuilderRegistry registry) 
            : base(registry)
        {
            Registry = registry;
        }

        public PipelineBuilderRegistry Registry { get; }

        public static TestPipelineBuilder<TIn> Create<TIn>()
        {
            var registry = new PipelineBuilderRegistry();
            var builder = new TestPipelineBuilder(registry);
            return new TestPipelineBuilder<TIn>(builder);
        }

        public static TestPipelineBuilder<TIn, TOut> Create<TIn, TOut>()
        {
            var registry = new PipelineBuilderRegistry();
            var builder = new TestPipelineBuilder(registry);
            return new TestPipelineBuilder<TIn, TOut>(builder);
        }
    }

    internal class TestPipelineBuilder<TIn> : DefaultPipelineBuilder<TIn>
    {
        private readonly PipelineBuilderRegistry registry;

        public TestPipelineBuilder(TestPipelineBuilder pipelineBuilder) 
            : base(pipelineBuilder)
        {
            registry = pipelineBuilder.Registry;
        }

        public HandlerRegistry Handlers => registry.Handlers;

        public DecoratorRegistry Decorators => registry.Decorators;
    }

    internal class TestPipelineBuilder<TIn, TOut> : DefaultPipelineBuilder<TIn, TOut>
    {
        private readonly PipelineBuilderRegistry registry;

        public TestPipelineBuilder(TestPipelineBuilder pipelineBuilder)
            : base(pipelineBuilder)
        {
            registry = pipelineBuilder.Registry;
        }

        public HandlerRegistry Handlers => registry.Handlers;

        public DecoratorRegistry Decorators => registry.Decorators;
    }

    internal class PipelineBuilderTestService 
    {
        public static MethodInfo GetHandlerMethod() => typeof(PipelineBuilderTestService)
            .GetMethod(nameof(PipelineBuilderTestService.Handle))!;

        public static MethodInfo[] GetHandlerMethods() => typeof(PipelineBuilderTestService)
            .GetMethods()
            .Where(m => m.Name 
                is nameof(PipelineBuilderTestService.Handle)
                or nameof(PipelineBuilderTestService.HandleAsync))
            .ToArray();

        [TestPipelineBuilder]
        public void Handle(int input) { }

        public Task<string> HandleAsync(int input, CancellationToken token) => Task.FromResult(input.ToString());
    }

    internal class TestPipelineBuilderAttribute : Attribute { }
}
