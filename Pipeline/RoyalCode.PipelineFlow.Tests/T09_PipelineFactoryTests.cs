using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RoyalCode.PipelineFlow.Configurations;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T09_PipelineFactoryTests
    {

        [Fact]
        public  void T01_SingleInputHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.AddHandlerMethodDefined(typeof(SingleInputHandler), "Handle");
                })
                .Create();

            var pipeline = factory.Create<SingleInput>();
            Assert.NotNull(pipeline);

            pipeline.Send(new SingleInput(1));
            Assert.Equal(1, SingleInputHandler.Value);
        }

        [Fact]
        public void T02_SingleInputOutputHandler()
        {
            var factory = PipelineFactory.Configure<ITestBus>()
                .ConfigurePipeline(builder =>
                {
                    builder.AddHandlerMethodDefined(typeof(SingleInputOutputHandler), "Handle");
                })
                .Create();

            var pipeline = factory.Create<SingleInputOutput, int>();
            Assert.NotNull(pipeline);

            var output = pipeline.Send(new SingleInputOutput(1));
            Assert.Equal(1, output);
            Assert.Equal(1, SingleInputHandler.Value);
        }
    }

    internal interface ITestBus { }

    #region T01
    public class SingleInput
    {
        public int Value { get; }

        public SingleInput(int value)
        {
            Value = value;
        }
    }

    public class SingleInputHandler
    {
        public static int Value = 0;

        public void Handle(SingleInput input)
        {
            Value = input.Value;
        }
    }
    #endregion

    #region T02
    public class SingleInputOutput
    {
        public int Value { get; }

        public SingleInputOutput(int value)
        {
            Value = value;
        }
    }

    public class SingleInputOutputHandler
    {
        public static int Value = 0;

        public int Handle(SingleInput input)
        {
            Value = input.Value;
            return Value;
        }
    }
    #endregion
}
