using RoyalCode.PipelineFlow.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void _01_Implements_Not_Generic()
        {
            var typeGeneric = typeof(IGenericResolution_Test_01);
            var typeConstructed = typeof(GenericResolution_Test_00<string>);

            Assert.True(typeConstructed.Implements(typeGeneric));
        }

        [Fact]
        public void _02_Implements_With_Generics()
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
