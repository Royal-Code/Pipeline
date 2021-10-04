using RoyalCode.PipelineFlow.Extensions;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T01_TypeExtensionsTests
    {
        [Fact]
        public void T01_Implements_Not_Generic()
        {
            var typeGeneric = typeof(IGenericResolution_Test_01);
            var typeConstructed = typeof(GenericResolution_Test_00<string>);

            Assert.True(typeConstructed.Implements(typeGeneric));
        }

        [Fact]
        public void T02_Implements_With_Generics()
        {
            var typeGeneric = typeof(IGenericResolution_Test_02<>);
            var typeConstructed = typeof(GenericResolution_Test_00<string>);

            Assert.True(typeConstructed.Implements(typeGeneric));
        }

        private interface IGenericResolution_Test_01 { }

        private interface IGenericResolution_Test_02<T> : IGenericResolution_Test_01 { }

        private class GenericResolution_Test_00<T> : IGenericResolution_Test_02<T> { }
    }
}
