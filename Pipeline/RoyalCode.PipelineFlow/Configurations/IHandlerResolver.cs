using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Configurations
{
    public interface IHandlerResolver
    {
        bool IsFallback { get; }

        HandlerDescription? TryResolve(Type inputType);

        HandlerDescription? TryResolve(Type inputType, Type output);
    }

    public abstract class HandlerResolverBase : IHandlerResolver
    {
        private readonly HandlerDescription handlerDescription;

        protected HandlerResolverBase(HandlerDescription handlerDescription)
        {
            this.handlerDescription = handlerDescription ?? throw new ArgumentNullException(nameof(handlerDescription));
        }

        public bool IsFallback { get; protected set; }

        public HandlerDescription? TryResolve(Type inputType)
        {
            return handlerDescription.Match(inputType)
                ? handlerDescription
                : null;
        }

        public HandlerDescription? TryResolve(Type inputType, Type output)
        {
            return handlerDescription.Match(inputType, output)
                ? handlerDescription
                : null;
        }
    }

    public class DelegateHandlerResolver : HandlerResolverBase
    {
        public DelegateHandlerResolver(Delegate handler)
            : base(handler.GetHandlerDescription())
        { }
    }

    public class ServiceAndDelegateHandlerResolver : HandlerResolverBase
    {
        public ServiceAndDelegateHandlerResolver(Delegate handler, Type serviceType)
            : base(handler.GetHandlerDescription(serviceType))
        { }
    }

    public class MethodHandlerResolver : HandlerResolverBase
    {
        public MethodHandlerResolver(MethodInfo methodHandler)
            : base(methodHandler.GetHandlerDescription())
        { }
    }

    public static class DefaultHandlersResolver
    {
        public static IHandlerResolver Handle<TInput>(Action<TInput> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TInput>(Func<TInput, Task> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TService, TInput>(Action<TService, TInput> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver Handle<TService, TInput>(Func<TService, TInput, Task> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver Handle<TInput, TOutput>(Func<TInput, TOutput> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TInput, TOutput>(Func<TInput, Task<TOutput>> handler)
            => new DelegateHandlerResolver(handler);

        public static IHandlerResolver Handle<TService, TInput, TOutput>(Func<TService, TInput, TOutput> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));

        public static IHandlerResolver Handle<TService, TInput, TOutput>(Func<TService, TInput, Task<TOutput>> handler)
            => new ServiceAndDelegateHandlerResolver(handler, typeof(TService));
    }
}
