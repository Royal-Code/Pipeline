using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T12_PipelineDispatchersTests
    {
        [Fact]
        public void T01_GetDispatcher_In()
        {
            TestDispatchers.Clear();
            var dispatchers = new TestDispatchers();

            var @delegate =  dispatchers.GetDispatcher(typeof(int));

            Assert.NotNull(@delegate);
        }

        [Fact]
        public void T02_GetDispatcher_InOut()
        {
            TestDispatchers.Clear();
            var dispatchers = new TestDispatchers();

            var @delegate = dispatchers.GetDispatcher<string>(typeof(int));

            Assert.NotNull(@delegate);
        }

        [Fact]
        public void T03_GetAsyncDispatcher_In()
        {
            TestDispatchers.Clear();
            var dispatchers = new TestDispatchers();

            var @delegate = dispatchers.GetAsyncDispatcher(typeof(int));

            Assert.NotNull(@delegate);
        }

        [Fact]
        public void T04_GetAsyncDispatcher_InOut()
        {
            TestDispatchers.Clear();
            var dispatchers = new TestDispatchers();

            var @delegate = dispatchers.GetAsyncDispatcher<string>(typeof(int));

            Assert.NotNull(@delegate);
        }
    }

    internal class TestDispatchers : PipelineDispatchers<TestDispatchers> 
    {
        public static void Clear() => ResetDispatchers();
    }
}
