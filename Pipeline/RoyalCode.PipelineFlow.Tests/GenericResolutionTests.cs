using RoyalCode.PipelineFlow.Configurations;
using System;
using System.Collections.Generic;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class GenericResolutionTests
    {
        [Fact]
        public void _01_List_T()
        {
            var method = typeof(GenericResolution_Test_01<>).GetMethod("Handler");

            var resolution = new GenericResolution(
                method.GetParameters()[0].ParameterType,
                method.ReturnType,
                false,
                false,
                method);

            Assert.NotNull(resolution);

            var @delegate = resolution.CreateDelegate(typeof(List<int>), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericResolution_Test_01<int>, List<int>>;
            Assert.NotNull(action);

            action(new GenericResolution_Test_01<int>(), new List<int>());
        }

        [Fact]
        public void _02_Tuple_T2_T1()
        {
            var method = typeof(GenericResolution_Test_02<,>).GetMethod("Handler");

            var resolution = new GenericResolution(
                method.GetParameters()[0].ParameterType,
                method.ReturnType,
                false,
                false,
                method);

            Assert.NotNull(resolution);

            var @delegate = resolution.CreateDelegate(typeof(Tuple<int, string>), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericResolution_Test_02<string, int>, Tuple<int, string>>;
            Assert.NotNull(action);

            action(new GenericResolution_Test_02<string, int>(), new Tuple<int, string>(0, string.Empty));
        }

        [Fact]
        public void _03_IsAssinableFrom()
        {
            var typeGeneric = typeof(IGenericResolution_Test_03);
            var typeConstructed = typeof(GenericResolution_Test_03<string>);

            Assert.True(typeGeneric.IsAssignableFrom(typeConstructed));

            // tem que dar um jeito disso batar como verdadeiro
            typeGeneric = typeof(IGenericResolution_Test_03<>);
            typeConstructed = typeConstructed.GetGenericTypeDefinition();

            Assert.True(typeGeneric.IsAssignableFrom(typeConstructed));
        }

        private class GenericResolution_Test_01<T>
        {
            public void Handler(List<T> input)
            {
                if (input is null)
                {
                    throw new ArgumentNullException(nameof(input));
                }
            }
        }

        private class GenericResolution_Test_02<T1, T2>
        {
            public void Handler(Tuple<T2, T1> input)
            {
                if (input is null)
                {
                    throw new ArgumentNullException(nameof(input));
                }
            }
        }

        private interface IGenericResolution_Test_03 { }

        private interface IGenericResolution_Test_03<T> : IGenericResolution_Test_03 { }

        private class GenericResolution_Test_03<T> : IGenericResolution_Test_03<T> { }
    }
}
