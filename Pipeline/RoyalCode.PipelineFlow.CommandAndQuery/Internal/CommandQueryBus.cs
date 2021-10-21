using RoyalCode.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.CommandAndQuery.Internal
{
    internal class CommandQueryBus : ICommandQueryBus
    {
        private readonly IPipelineFactory<ICommandQueryBus> factory;
        private readonly RequestDispatchers dispatchers;

        public CommandQueryBus(IPipelineFactory<ICommandQueryBus> factory, RequestDispatchers dispatchers)
        {
            this.factory = factory;
            this.dispatchers = dispatchers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send(IRequest request)
            => dispatchers.GetDispatcher(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))(request, factory);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult Send<TResult>(IRequest<TResult> request)
            => dispatchers.GetDispatcher<TResult>(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))(request, factory);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task SendAsync(IRequest request, CancellationToken token = default)
            => dispatchers.GetAsyncDispatcher(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))(request, factory);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token = default)
            => dispatchers.GetAsyncDispatcher<TResult>(request?.GetType() ?? throw new ArgumentNullException(nameof(request)))(request, factory);
    }

    internal class RequestDispatchers
    {
        private Dictionary<Type, object> dispatchers = new();
        private Dictionary<Type, object> asyncDispatchers = new();

        private static readonly MethodInfo requestDispatcherMethod = typeof(RequestDispatchers)
            .GetMethods()
            .Where(m => m.Name == nameof(RequestDispatchers.GetDispatcher))
            .Where(m => m.GetGenericArguments().Length == 1)
            .First();

        private static readonly MethodInfo requestResultDispatcherMethod = typeof(RequestDispatchers)
            .GetMethods()
            .Where(m => m.Name == nameof(RequestDispatchers.GetDispatcher))
            .Where(m => m.GetGenericArguments().Length == 2)
            .First();

        private static readonly MethodInfo requestAsyncDispatcherMethod = typeof(RequestDispatchers)
            .GetMethods()
            .Where(m => m.Name == nameof(RequestDispatchers.GetAsyncDispatcher))
            .Where(m => m.GetGenericArguments().Length == 1)
            .First();

        private static readonly MethodInfo requestResultAsyncDispatcherMethod = typeof(RequestDispatchers)
            .GetMethods()
            .Where(m => m.Name == nameof(RequestDispatchers.GetAsyncDispatcher))
            .Where(m => m.GetGenericArguments().Length == 2)
            .First();

        public Action<IRequest, IPipelineFactory<ICommandQueryBus>> GetDispatcher(Type requestType)
        {
            return (Action<IRequest, IPipelineFactory<ICommandQueryBus>>) requestDispatcherMethod
                .MakeGenericMethod(requestType)
                .Invoke(this, new object[0]);
        }

        public Func<IRequest<TOut>, IPipelineFactory<ICommandQueryBus>, TOut> GetDispatcher<TOut>(Type requestType)
        {
            return (Func<IRequest<TOut>, IPipelineFactory<ICommandQueryBus>, TOut>)requestResultDispatcherMethod
                .MakeGenericMethod(requestType, typeof(TOut))
                .Invoke(this, new object[0]);
        }

        public Func<IRequest, IPipelineFactory<ICommandQueryBus>, Task> GetAsyncDispatcher(Type requestType)
        {
            return (Func<IRequest, IPipelineFactory<ICommandQueryBus>, Task>)requestAsyncDispatcherMethod
                .MakeGenericMethod(requestType)
                .Invoke(this, new object[0]);
        }

        public Func<IRequest<TOut>, IPipelineFactory<ICommandQueryBus>, Task<TOut>> GetAsyncDispatcher<TOut>(Type requestType)
        {
            return (Func<IRequest<TOut>, IPipelineFactory<ICommandQueryBus>, Task<TOut>>)requestResultAsyncDispatcherMethod
                .MakeGenericMethod(requestType, typeof(TOut))
                .Invoke(this, new object[0]);
        }

        private Action<IRequest, IPipelineFactory<ICommandQueryBus>> GetDispatcher<TIn>()
            where TIn : class, IRequest
        {
            return (request, factory) =>
            {
                var pipeline = factory.Create<TIn>();
                pipeline.Send((TIn)request);
            };
        }

        private Func<IRequest<TOut>, IPipelineFactory<ICommandQueryBus>, TOut> GetDispatcher<TIn, TOut>()
            where TIn : class, IRequest<TOut>
        {
            return (request, factory) =>
            {
                var pipeline = factory.Create<TIn, TOut>();
                return pipeline.Send((TIn)request);
            };
        }

        private Func<IRequest, IPipelineFactory<ICommandQueryBus>, Task> GetAsyncDispatcher<TIn>()
        {
            return (request, factory) =>
            {
                var pipeline = factory.Create<TIn>();
                return pipeline.SendAsync((TIn)request);
            };
        }

        private Func<IRequest<TOut>, IPipelineFactory<ICommandQueryBus>, Task<TOut>> GetAsyncDispatcher<TIn, TOut>()
            where TIn : class, IRequest<TOut>
        {
            return (request, factory) =>
            {
                var pipeline = factory.Create<TIn, TOut>();
                return pipeline.SendAsync((TIn)request);
            };
        }
    }
}
