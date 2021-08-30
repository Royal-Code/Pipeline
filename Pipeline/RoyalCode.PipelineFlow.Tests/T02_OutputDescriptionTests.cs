using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T02_OutputDescriptionTests
    {
        [Fact]
        public void T01_Void()
        {
            var method = typeof(OutputDescriptionTests_01).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescription(method!);

            Assert.False(output.HasOutput);
            Assert.False(output.IsAsync);
            Assert.True(output.IsVoid);
            Assert.Equal(typeof(void), output.OutputType);
        }

        [Fact]
        public void T02_String()
        {
            var method = typeof(OutputDescriptionTests_02).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescription(method!);

            Assert.True(output.HasOutput);
            Assert.False(output.IsAsync);
            Assert.False(output.IsVoid);
            Assert.Equal(typeof(string), output.OutputType);
        }

        [Fact]
        public void T03_Task()
        {
            var method = typeof(OutputDescriptionTests_03).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescription(method!);

            Assert.False(output.HasOutput);
            Assert.True(output.IsAsync);
            Assert.False(output.IsVoid);
            Assert.Equal(typeof(Task), output.OutputType);
        }

        [Fact]
        public void T04_String()
        {
            var method = typeof(OutputDescriptionTests_04).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescription(method!);

            Assert.True(output.HasOutput);
            Assert.True(output.IsAsync);
            Assert.False(output.IsVoid);
            Assert.Equal(typeof(string), output.OutputType);
        }

        private class OutputDescriptionTests_01
        {
            public void Handler(int input) { }
        }

        private class OutputDescriptionTests_02
        {
            public string Handler(int input) => "ok";
        }

        private class OutputDescriptionTests_03
        {
            public Task Handler(int input) => Task.CompletedTask;
        }

        private class OutputDescriptionTests_04
        {
            public Task<string> Handler(int input) => Task.FromResult("ok");
        }
    }
}
