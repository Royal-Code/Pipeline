using RoyalCode.PipelineFlow.EventDispatcher.Exceptions;
using RoyalCode.PipelineFlow.EventDispatcher.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.EventDispatcher.Tests;

public class T01_ObserverMethodResolverTest
{
    [Theory]
    [InlineData(nameof(ReturnTypesModel.AsyncMethod), true)]
    [InlineData(nameof(ReturnTypesModel.SyncMethod), false)]
    public void AcceptedReturnTypes(string methodName, bool isAsync)
    {
        var method = typeof(ReturnTypesModel).GetMethod(methodName)
            ?? throw new Exception();

        var resolver = new ObserverMethodResolver(method);

        Assert.Equal(isAsync, resolver.IsAsync());
    }

    [Theory]
    [InlineData(nameof(ReturnTypesModel.AsyncInvalidMethod))]
    [InlineData(nameof(ReturnTypesModel.SyncInvalidMethod))]
    public void InvalidReturnTypes(string methodName)
    {
        var method = typeof(ReturnTypesModel).GetMethod(methodName)
            ?? throw new Exception();

        Assert.ThrowsAny<InvalidObserverMethodException>(() => _ = new ObserverMethodResolver(method));
    }

    [Theory]
    [InlineData(nameof(InvalidMethodsModel.NoArgMethod))]
    [InlineData(nameof(InvalidMethodsModel.NoClassEventTypeMethod))]
    [InlineData(nameof(InvalidMethodsModel.ToManyArgsMethod1))]
    [InlineData(nameof(InvalidMethodsModel.ToManyArgsMethod2))]
    [InlineData(nameof(InvalidMethodsModel.InvalidSecondArgMethod))]
    public void InvalidParameters(string methodName)
    {
        var method = typeof(InvalidMethodsModel).GetMethod(methodName)
            ?? throw new Exception();

        Assert.ThrowsAny<InvalidObserverMethodException>(() => _ = new ObserverMethodResolver(method));
    }

    [Theory]
    [InlineData(nameof(ValidMethodsModel.SyncMethod))]
    [InlineData(nameof(ValidMethodsModel.AsyncMethod))]
    [InlineData(nameof(ValidMethodsModel.AsyncMethodWithToken))]
    public void CreateDelegatesForValidMethods(string methodName)
    {
        var method = typeof(ValidMethodsModel).GetMethod(methodName)
            ?? throw new Exception();

        var resolver = new ObserverMethodResolver(method);

        var func = resolver.BuildCallerFunction();

        // Func<TService, TEvent, CancellationToken, Task>
        var funcType = typeof(Func<,,,>)
            .MakeGenericType(typeof(ValidMethodsModel), typeof(object), typeof(CancellationToken), typeof(Task));

        Assert.Equal(funcType, func.GetType());
    }

    private class ValidMethodsModel
    {
        public void SyncMethod(object e) { }

        public Task AsyncMethod(object e) { throw new Exception(); }

        public Task AsyncMethodWithToken(object e, CancellationToken token) { throw new Exception(); }
    }

    private class InvalidMethodsModel
    {
        public void NoArgMethod() { }

        public void NoClassEventTypeMethod(int i) { throw new Exception(); }

        public void ToManyArgsMethod1(object e, CancellationToken token) { }

        public Task ToManyArgsMethod2(object e, CancellationToken token, int i) { throw new Exception(); }

        public Task InvalidSecondArgMethod(object e, int i) { throw new Exception(); }
    }

    private class ReturnTypesModel
    {
        public void SyncMethod(object e) { }

        public Task AsyncMethod(object e) { throw new Exception(); }

        public Task<object> AsyncInvalidMethod(object e) { throw new Exception(); }

        public object SyncInvalidMethod(object e) { throw new Exception(); }
    }
}

