using RoyalCode.PipelineFlow.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.PipelineFlow.Tests
{
    public class T03_GenericResolutionTests
    {
        [Fact]
        public void T01_List_T()
        {
            var method = typeof(GenericResolution_Test_01<>).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method);

            var @delegate = resolution.CreateDelegate(typeof(List<int>), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericResolution_Test_01<int>, List<int>>;
            Assert.NotNull(action);

            action!(new GenericResolution_Test_01<int>(), new List<int>());
        }

        [Fact]
        public void T02_Tuple_T2_T1()
        {
            var method = typeof(GenericResolution_Test_02<,>).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(Tuple<int, string>), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericResolution_Test_02<string, int>, Tuple<int, string>>;
            Assert.NotNull(action);

            action!(new GenericResolution_Test_02<string, int>(), new Tuple<int, string>(0, string.Empty));
        }

        [Fact]
        public void T03_Simple_Int32()
        {
            var method = typeof(GenericResolution_Test_03).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericResolution_Test_03, int>;
            Assert.NotNull(action);

            action!(new GenericResolution_Test_03(), 1);
        }

        [Fact]
        public void T04_Simple_Task_Int32()
        {
            var method = typeof(GenericResolution_Test_04).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(Task));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_04, int, Task>;
            Assert.NotNull(function);

            var result = function!(new GenericResolution_Test_04(), 1);
            Assert.NotNull(result);
        }

        [Fact]
        public void T05_Simple_Task_WithResult_Int32()
        {
            var method = typeof(GenericResolution_Test_05).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(int));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_05, int, Task<int>>;
            Assert.NotNull(function);

            var result = function!(new GenericResolution_Test_05(), 1);
            Assert.NotNull(result);

            var value = result.Result;
            Assert.Equal(1, value);
        }

        [Fact]
        public void T06_GenericResult_Int32_TResult()
        {
            var method = typeof(GenericResolution_Test_06<>).GetMethod("Handler");
            Assert.NotNull(method); 
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(int));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_06<int>, int, int>;
            Assert.NotNull(function);

            var result = function!(new GenericResolution_Test_06<int>(), 1);
            Assert.Equal(0, result);
        }

        [Fact]
        public void T07_GenericResult_Int32_Task_TResult()
        {
            var method = typeof(GenericResolution_Test_07<>).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(int));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_07<int>, int, Task<int>>;
            Assert.NotNull(function);

            var result = function!(new GenericResolution_Test_07<int>(), 1);
            Assert.NotNull(result);

            var value = result.Result;
            Assert.Equal(default, value);
        }

        [Fact]
        public void T08_GenericResult_Int32_List_TResult()
        {
            var method = typeof(GenericResolution_Test_08<>).GetMethod("Handler");
            Assert.NotNull(method); 
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(List<int>));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_08<int>, int, List<int>>;
            Assert.NotNull(function);

            var result = function!(new GenericResolution_Test_08<int>(), 1);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void T09_GenericResult_Int32_Task_List_TResult()
        {
            var method = typeof(GenericResolution_Test_09<>).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(List<int>));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_09<int>, int, Task<List<int>>>;
            Assert.NotNull(function);

            var result = function!(new GenericResolution_Test_09<int>(), 1);
            Assert.NotNull(result);

            var list = result.Result;
            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public void T10_GenericResult_T_List_T()
        {
            var method = typeof(GenericResolution_Test_10<>).GetMethod("Handler");
            Assert.NotNull(method); 
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(List<int>));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_10<int>, int, List<int>>;
            Assert.NotNull(function);

            var result = function!(new GenericResolution_Test_10<int>(), 1);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result[0]);
        }

        [Fact]
        public void T11_GenericResult_T_Task_List_T()
        {
            var method = typeof(GenericResolution_Test_11<>).GetMethod("Handler");
            Assert.NotNull(method); 
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(List<int>));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_11<int>, int, Task<List<int>>>;
            Assert.NotNull(function);

            var result = function!(new GenericResolution_Test_11<int>(), 1);
            Assert.NotNull(result);

            var list = result.Result;
            Assert.NotNull(list);
            Assert.NotEmpty(list);
            Assert.Equal(1, list[0]);
        }

        [Fact]
        public void T12_Simple_Input()
        {
            
            var method = typeof(GenericResolution_Test_12<>).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericResolution_Test_12<int>, int>;
            Assert.NotNull(action);

            action!(new GenericResolution_Test_12<int>(), 1);
        }

        [Fact]
        public void T13_Simple_Input_Simple_Next()
        {

            var method = typeof(GenericResolution_Test_13<>).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(void));
            Assert.NotNull(@delegate);

            var action = @delegate as Action<GenericResolution_Test_13<int>, int, Action<string>>;
            Assert.NotNull(action);

            action!(new GenericResolution_Test_13<int>(), 1, s => { });
        }

        [Fact]
        public void T14_GenericResult_Input_Next_GenericResult()
        {
            var method = typeof(GenericResolution_Test_14<,>).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(long));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_14<int, long>, int, Func<string, long>, long>;
            Assert.NotNull(function);

            function!(new GenericResolution_Test_14<int, long>(), 1, s => 1L);
        }

        [Fact]
        public void T15_GenericResult_Input_Next_GenericResult_Async()
        {
            var method = typeof(GenericResolution_Test_15<,>).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(long));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_15<int, long>, int, Func<string, Task<long>>, Task<long>>;
            Assert.NotNull(function);

            function!(new GenericResolution_Test_15<int, long>(), 1, s => Task.FromResult(1L));
        }

        [Fact]
        public void T16_GenericResult_Input_Next_Complex_Result_Async()
        {
            var method = typeof(GenericResolution_Test_16<,>).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(long));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_16<int, long>, int, Func<string, Task<Tuple<List<long>>>>, Task<long>>;
            Assert.NotNull(function);

            function!(new GenericResolution_Test_16<int, long>(), 1, s => Task.FromResult(new Tuple<List<long>>(new List<long>() { 1L })));
        }

        [Fact]
        public void T17_GenericResult_Input_Next_GenericResult_Async_WithToken()
        {
            var method = typeof(GenericResolution_Test_17<,>).GetMethod("Handler");
            Assert.NotNull(method);
            var output = new OutputDescriptor(method!);

            var resolution = new GenericResolution(
                method!.GetParameters()[0].ParameterType,
                output.OutputType,
                output.IsAsync,
                output.HasOutput,
                method!);

            var @delegate = resolution.CreateDelegate(typeof(int), typeof(long));
            Assert.NotNull(@delegate);

            var function = @delegate as Func<GenericResolution_Test_17<int, long>, int, Func<string, Task<long>>, CancellationToken, Task<long>>;
            Assert.NotNull(function);

            function!(new GenericResolution_Test_17<int, long>(), 1, s => Task.FromResult(1L), default);
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

        private class GenericResolution_Test_03
        {
            public void Handler(int input)
            {
                if (input <= 0)
                {
                    throw new InvalidOperationException("input must be positive");
                }
            }
        }

        private class GenericResolution_Test_04
        {
            public Task Handler(int input)
            {
                if (input <= 0)
                {
                    throw new InvalidOperationException("input must be positive");
                }
                return Task.CompletedTask;
            }
        }

        private class GenericResolution_Test_05
        {
            public Task<int> Handler(int input)
            {
                if (input <= 0)
                {
                    throw new InvalidOperationException("input must be positive");
                }
                return Task.FromResult(input);
            }
        }

        private class GenericResolution_Test_06<T>
        {
            public T? Handler(int input)
            {
                if (input <= 0)
                {
                    throw new InvalidOperationException("input must be positive");
                }
                return default;
            }
        }

        private class GenericResolution_Test_07<T>
        {
            public Task<T> Handler(int input)
            {
                if (input <= 0)
                {
                    throw new InvalidOperationException("input must be positive");
                }
                return Task.FromResult<T>(default!);
            }
        }

        private class GenericResolution_Test_08<T>
        {
            public List<T> Handler(int input)
            {
                if (input <= 0)
                {
                    throw new InvalidOperationException("input must be positive");
                }
                return new List<T>();
            }
        }

        private class GenericResolution_Test_09<T>
        {
            public Task<List<T>> Handler(int input)
            {
                if (input <= 0)
                {
                    throw new InvalidOperationException("input must be positive");
                }
                return Task.FromResult(new List<T>());
            }
        }

        private class GenericResolution_Test_10<T>
        {
            public List<T> Handler(T input)
            {
                if (Equals(input, default(T)))
                {
                    throw new InvalidOperationException("input must be informed");
                }
                return new List<T>() { input };
            }
        }

        private class GenericResolution_Test_11<T>
        {
            public Task<List<T>> Handler(T input)
            {
                if (Equals(input, default(T)))
                {
                    throw new InvalidOperationException("input must be informed");
                }
                return Task.FromResult(new List<T>() { input });
            }
        }

        private class GenericResolution_Test_12<T>
        {
            public void Handler(T input)
            {
                if (Equals(input, default(T)))
                {
                    throw new InvalidOperationException("input must be informed");
                }
            }
        }

        private class GenericResolution_Test_13<T>
        {
            public void Handler(T input, Action<string> next)
            {
                if (Equals(input, default(T)))
                {
                    throw new InvalidOperationException("input must be informed");
                }

                if (next is null)
                    throw new InvalidOperationException("input must be informed");
            }
        }

        private class GenericResolution_Test_14<T1, T2>
        {
            public T2 Handler(T1 input, Func<string, T2> next)
            {
                if (Equals(input, default(T1)))
                {
                    throw new InvalidOperationException("input must be informed");
                }

                return next("1");
            }
        }

        private class GenericResolution_Test_15<T1, T2>
        {
            public Task<T2?> Handler(T1 input, Func<string, Task<T2?>> next)
            {
                if (Equals(input, default(T1)))
                {
                    throw new InvalidOperationException("input must be informed");
                }

                return next("1");
            }
        }

        private class GenericResolution_Test_16<T1, T2>
        {
            public async Task<T2?> Handler(T1 input, Func<string, Task<Tuple<List<T2>>>> next)
            {
                if (Equals(input, default(T1)))
                {
                    throw new InvalidOperationException("input must be informed");
                }

                var result = await next("1");

                return result.Item1.FirstOrDefault();
            }
        }

        private class GenericResolution_Test_17<T1, T2>
        {
            public Task<T2?> Handler(T1 input, Func<string, Task<T2?>> next, CancellationToken token)
            {
                if (Equals(input, default(T1)))
                {
                    throw new InvalidOperationException("input must be informed");
                }

                return next("1");
            }
        }
    }
}
