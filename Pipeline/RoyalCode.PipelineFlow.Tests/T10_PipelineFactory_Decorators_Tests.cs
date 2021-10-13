using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T10_PipelineFactory_Decorators_Tests
    {
        [Fact]
        public void T01_DecoratorsHandler_In()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipelines(builder =>
                {
                    builder.AddHandlersMethods(MethodsGetter<ValueableHandler<IntInput, int>>.GetSyncMethods());
                    builder.AddDecoratorsMethods(MethodsGetter<DecoratorHandler_In>.GetSyncMethods());
                })
                .Create();

            var pipeline = factory.Create<IntInput>();
            Assert.NotNull(pipeline);

            pipeline.Send(new IntInput(1));
            Assert.Equal(4, ValueableHandler<IntInput, int>.Value);
        }
    }

    #region Input & Handlers

    public interface IValueable<TValue>
    {
        TValue Value { get; }
        void Increment();
    }

    public class IntInput : IValueable<int>
    {
        public int Value { get; set; }

        public IntInput(int value)
        {
            Value = value;
        }

        public void Increment()
        {
            Value++;
        }
    }

    public class ValueableHandler<TValueable, TValue>
        where TValueable : IValueable<TValue>
    {
        public static TValue? Value;

        public void Handle(TValueable input)
        {
            input.Increment();
            Value = input.Value;
        }

        public Task HandleAsync(TValueable input)
        {
            input.Increment();
            Value = input.Value;
            return Task.CompletedTask;
        }
    }

    public class MethodsGetter<T>
    {
        public static MethodInfo[] GetSyncMethods() => typeof(T)
            .GetMethods()
            .Where(m => m.Name.Contains("Handle", StringComparison.OrdinalIgnoreCase)
                && !m.Name.Contains("Async", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        public static MethodInfo[] GetAsyncMethods() => typeof(T)
            .GetMethods()
            .Where(m => m.Name.Contains("Handle", StringComparison.OrdinalIgnoreCase)
                && m.Name.Contains("Async", StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    #endregion

    #region T01

    public class DecoratorHandler_In
    {
        public void Handle1(IntInput input, Action next)
        {
            input.Increment();
            next();
        }

        public void Handle2(IntInput input, Action next)
        {
            input.Increment();
            next();
        }
    }

    #endregion
}
