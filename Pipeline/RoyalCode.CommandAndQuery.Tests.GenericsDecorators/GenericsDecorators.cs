namespace RoyalCode.CommandAndQuery.Tests.GenericsDecorators
{

    public class GenericDecoratorIn<TIn> : IDecorator<TIn>
    {
        public void Handle(TIn request, Action next)
        {
            throw new NotImplementedException();
        }
    }

    public class GenericDecoratorInOut<TIn, TOut> : IDecorator<TIn, TOut>
    {
        public TOut Handle(TIn request, Func<TOut> next)
        {


            return next();
        }
    }
}