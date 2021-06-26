using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// Abstração para decoradores de pipeline.
    /// </summary>
    /// <typeparam name="TIn">Tipo de entrada.</typeparam>
    public abstract class DecoratorChain<TIn>
    {
        /// <summary>
        /// Envia uma requisição para ser processada por um manipulador que decora o pipeline.
        /// </summary>
        /// <param name="input">Objeto de entrada para processamento.</param>
        /// <param name="next">Ação para executar o próximo handler do pipeline.</param>
        public abstract void Send(TIn input, Action next);

        /// <summary>
        /// Envia uma requisição para ser processada por um manipulador que decora o pipeline.
        /// </summary>
        /// <param name="input">Objeto de entrada para processamento.</param>
        /// <param name="next">Ação para executar o próximo handler do pipeline.</param>
        /// <param name="token">Token para cancelamento.</param>
        /// <returns>Resultado do processamento da requisição</returns>
        public abstract Task SendAsync(TIn input, Func<Task> next, CancellationToken token);
    }

    /// <summary>
    /// Abstração para decoradores de pipeline.
    /// </summary>
    /// <typeparam name="TIn">Tipo de entrada.</typeparam>
    /// <typeparam name="TOut">Tipo de saída.</typeparam>
    public abstract class DecoratorChain<TIn, TOut>
    {
        /// <summary>
        /// Envia uma requisição para ser processada por um manipulador que decora o pipeline.
        /// </summary>
        /// <param name="input">Objeto de entrada para processamento.</param>
        /// <param name="next">Ação para executar o próximo handler do pipeline.</param>
        public abstract TOut Send(TIn input, Func<TOut> next);

        /// <summary>
        /// Envia uma requisição para ser processada por um manipulador que decora o pipeline.
        /// </summary>
        /// <param name="input">Objeto de entrada para processamento.</param>
        /// <param name="next">Ação para executar o próximo handler do pipeline.</param>
        /// <param name="token">Token para cancelamento.</param>
        /// <returns>Resultado do processamento da requisição</returns>
        public abstract Task<TOut> SendAsync(TIn input, Func<Task<TOut>> next, CancellationToken token);
    }
}
