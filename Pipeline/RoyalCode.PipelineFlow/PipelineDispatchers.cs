using RoyalCode.PipelineFlow.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow
{
    /// <summary>
    /// <para>
    ///     Dispatcher are delegated to get a pipeline of an input type and send them out for processing.
    /// </para>
    /// <para>
    ///     This is a generic component for performing this operation.
    /// </para>
    /// </summary>
    /// <typeparam name="TFor">The pipeline type.</typeparam>
    public class PipelineDispatchers<TFor>
    {
        private static readonly Dictionary<Type, object> dispatchers = new();
        private static readonly Dictionary<Type, object> asyncDispatchers = new();

        internal static readonly MethodInfo requestDispatcherMethod = typeof(PipelineDispatchers<TFor>)
            .GetTypeInfo()
            .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(m => m.Name == nameof(PipelineDispatchers<object>.GetDispatcher))
            .Where(m => m.GetGenericArguments().Length == 1)
            .First();

        internal static readonly MethodInfo requestResultDispatcherMethod = typeof(PipelineDispatchers<TFor>)
            .GetTypeInfo()
            .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(m => m.Name == nameof(PipelineDispatchers<object>.GetDispatcher))
            .Where(m => m.GetGenericArguments().Length == 2)
            .First();

        internal static readonly MethodInfo requestAsyncDispatcherMethod = typeof(PipelineDispatchers<TFor>)
            .GetTypeInfo()
            .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(m => m.Name == nameof(PipelineDispatchers<object>.GetAsyncDispatcher))
            .Where(m => m.GetGenericArguments().Length == 1)
            .First();

        internal static readonly MethodInfo requestResultAsyncDispatcherMethod = typeof(PipelineDispatchers<TFor>)
            .GetTypeInfo()
            .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(m => m.Name == nameof(PipelineDispatchers<object>.GetAsyncDispatcher))
            .Where(m => m.GetGenericArguments().Length == 2)
            .First();

        /// <summary>
        /// It gets a synchronous dispatch delegate without producing a result.
        /// </summary>
        /// <param name="requestType">The type of the request, is the input type.</param>
        /// <returns>The delegate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Action<object, IPipelineFactory<TFor>> GetDispatcher(Type requestType)
        {
            return (Action<object, IPipelineFactory<TFor>>)
                dispatchers.GetOrCreate(requestType, requestDispatcherMethod);
        }

        /// <summary>
        /// Get a synchronous dispatch delegate that produces a result.
        /// </summary>
        /// <typeparam name="TOut">Type of result to be produced.</typeparam>
        /// <param name="requestType">The type of the request, is the input type.</param>
        /// <returns>The delegate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func<object, IPipelineFactory<TFor>, TOut> GetDispatcher<TOut>(Type requestType)
        {
            return (Func<object, IPipelineFactory<TFor>, TOut>)
                dispatchers.GetOrCreate(requestType, typeof(TOut), requestResultDispatcherMethod);
        }

        /// <summary>
        /// Gets an asynchronous dispatch delegate without producing a result.
        /// </summary>
        /// <param name="requestType">The type of the request, is the input type.</param>
        /// <returns>The delegate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func<object, IPipelineFactory<TFor>, Task> GetAsyncDispatcher(Type requestType)
        {
            return (Func<object, IPipelineFactory<TFor>, Task>)
                dispatchers.GetOrCreate(requestType, requestAsyncDispatcherMethod);
        }

        /// <summary>
        /// Get an asynchronous dispatch delegate that produces a result.
        /// </summary>
        /// <typeparam name="TOut">Type of result to be produced.</typeparam>
        /// <param name="requestType">The type of the request, is the input type.</param>
        /// <returns>The delegate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Func<object, IPipelineFactory<TFor>, Task<TOut>> GetAsyncDispatcher<TOut>(Type requestType)
        {
            return (Func<object, IPipelineFactory<TFor>, Task<TOut>>)
                dispatchers.GetOrCreate(requestType, typeof(TOut), requestResultAsyncDispatcherMethod);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Action<object, IPipelineFactory<TFor>> GetDispatcher<TIn>()
        {
            return (request, factory) =>
            {
                var pipeline = factory.Create<TIn>();
                pipeline.Send((TIn)request);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Func<object, IPipelineFactory<TFor>, TOut> GetDispatcher<TIn, TOut>()
        {
            return (request, factory) =>
            {
                var pipeline = factory.Create<TIn, TOut>();
                return pipeline.Send((TIn)request);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Func<object, IPipelineFactory<TFor>, Task> GetAsyncDispatcher<TIn>()
        {
            return (request, factory) =>
            {
                var pipeline = factory.Create<TIn>();
                return pipeline.SendAsync((TIn)request);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Func<object, IPipelineFactory<TFor>, Task<TOut>> GetAsyncDispatcher<TIn, TOut>()
        {
            return (request, factory) =>
            {
                var pipeline = factory.Create<TIn, TOut>();
                return pipeline.SendAsync((TIn)request);
            };
        }
    }
}
