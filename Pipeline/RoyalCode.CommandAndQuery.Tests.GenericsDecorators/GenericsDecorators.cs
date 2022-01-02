namespace RoyalCode.CommandAndQuery.Tests.GenericsDecorators
{

    public class GenericDecoratorIn<TIn> : IDecorator<TIn>
    {
        private Action<TIn>? interceptor;

        public GenericDecoratorIn(Action<TIn>? interceptor = null)
        {
            this.interceptor = interceptor;
        }

        public void Handle(TIn request, Action next)
        {
            interceptor?.Invoke(request);
            next();
        }
    }

    public class GenericDecoratorInOut<TIn, TOut> : IDecorator<TIn, TOut>
    {
        private Action<TIn>? interceptor;
        private Action<TOut>? outterceptor;

        public GenericDecoratorInOut(Action<TIn>? interceptor = null, Action<TOut>? outterceptor = null)
        {
            this.interceptor = interceptor;
            this.outterceptor = outterceptor;
        }

        public TOut Handle(TIn request, Func<TOut> next)
        {
            interceptor?.Invoke(request);

            var output = next();

            outterceptor?.Invoke(output);

            return output;
        }
    }
}