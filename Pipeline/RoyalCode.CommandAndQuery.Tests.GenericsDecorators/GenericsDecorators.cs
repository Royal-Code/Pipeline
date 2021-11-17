namespace RoyalCode.CommandAndQuery.Tests.GenericsDecorators
{

    public class GenericDecoratorIn<TIn> : IDecorator<TIn>
    {
        public void Handle(TIn request, Action next)
        {
            throw new NotImplementedException();
        }
    }
}