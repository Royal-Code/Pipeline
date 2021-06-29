using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalCode.PipelineFlow.Chains
{
    /// <summary>
    /// Abstração de um Chain do Pipeline.
    /// </summary>
    /// <typeparam name="TIn">Tipo de entrada.</typeparam>
    public abstract class Chain<TIn> : IPipeline<TIn>
    {
        /// <summary>
        /// Envia uma requisição para ser processada por um manipulador.
        /// </summary>
        /// <param name="input">Objeto de entrada para processamento.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public abstract void Send(TIn input);

        /// <summary>
        /// Envia uma requisição para ser processada por um manipulador.
        /// </summary>
        /// <param name="input">Objeto de entrada para processamento.</param>
        /// <param name="token">Token para cancelamento.</param>
        /// <returns>Resultado do processamento da requisição</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public abstract Task SendAsync(TIn input, CancellationToken token);
    }

    /// <summary>
    /// Abstração de um Chain do Pipeline.
    /// </summary>
    /// <typeparam name="TIn">Tipo de entrada.</typeparam>
    /// <typeparam name="TOut">Tipo do resultado, saída.</typeparam>
    public abstract class Chain<TIn, TOut> : IPipeline<TIn, TOut>
    {
        /// <summary>
        /// Envia uma requisição para ser processada por um manipulador.
        /// </summary>
        /// <param name="input">Objeto de entrada para processamento.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public abstract TOut Send(TIn input);

        /// <summary>
        /// Envia uma requisição para ser processada por um manipulador.
        /// </summary>
        /// <param name="input">Objeto de entrada para processamento.</param>
        /// <param name="token">Token para cancelamento.</param>
        /// <returns>Resultado do processamento da requisição</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public abstract Task<TOut> SendAsync(TIn input, CancellationToken token);
    }
}
