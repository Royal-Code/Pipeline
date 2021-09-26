//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace RoyalCode.PipelineFlow.Configurations
//{
//    #region Handlers Delegates

//    public delegate void InputHandlerSync<TInput>(TInput input);

//    public delegate Task InputHandlerAsync<TInput>(TInput input, CancellationToken cancellationToken);

//    public delegate Task InputHandlerAsyncWithoutCancellationToken<TInput>(TInput input);



//    public delegate TOutput InputOutputHandlerSync<TInput, TOutput>(TInput input);

//    public delegate Task<TOutput> InputOutputHandlerAsync<TInput, TOutput>(TInput input, CancellationToken cancellationToken);

//    public delegate Task<TOutput> InputOutputHandlerAsyncWithoutCancellationToken<TInput, TOutput>(TInput input);



//    public delegate void ServiceInputHandlerSync<TService, TInput>(TService service, TInput input);

//    public delegate Task ServiceInputHandlerAsync<TService, TInput>(TService service, TInput input, CancellationToken cancellationToken);

//    public delegate Task ServiceInputHandlerAsyncWithoutCancellationToken<TService, TInput>(TService service, TInput input);



//    public delegate TOutput ServiceInputOutputHandlerSync<TService, TInput, TOutput>(TService service, TInput input);

//    public delegate Task<TOutput> ServiceInputOutputHandlerAsync<TService, TInput, TOutput>(TService service, TInput input, CancellationToken cancellationToken);

//    public delegate Task<TOutput> ServiceInputOutputHandlerAsyncWithoutCancellationToken<TService, TInput, TOutput>(TService service, TInput input);

//    #endregion

//    #region Bridges Delegates

//    public delegate void InputBridgeHandlerSync<TInput, TNextInput>(TInput input, Action<TNextInput> next);

//    public delegate Task InputBridgeHandlerAsync<TInput>(TInput input, CancellationToken cancellationToken);

//    public delegate Task InputBridgeHandlerAsyncWithoutCancellationToken<TInput>(TInput input);

//    #endregion
//}