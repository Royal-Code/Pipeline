using System;

namespace RoyalCode.CommandAndQuery
{
    /// <summary>
    /// <para>
    ///     Decorators for pipeline requests.
    ///     These objects are executed by the <see cref="ICommandQueryBus"/>.
    /// </para>
    /// <para>
    ///     The decorator adds a behavior to the pipeline requests,
    ///     it may modify the input value, perform some operation, 
    ///     stop or break the pipeline execution, or return a result value for request.
    /// </para>
    /// <para>
    ///     After the decorator behavior has been performed, the next handler should be called, 
    ///     otherwise execution will be interrupted.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Data type of the request.</typeparam>
    public interface IDecorator<in TRequest>
    {
        /// <summary>
        /// Pipeline decorator handler.
        /// Perform any additional behavior and call <paramref name="next"/> delegate as necessary.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="next">
        ///     Delegate for the next action in the pipeline.
        /// </param>
        void Handle(TRequest request, Action next);
    }

    /// <summary>
    /// <para>
    ///     Decorators for pipeline requests.
    ///     These objects are executed by the <see cref="ICommandQueryBus"/>.
    /// </para>
    /// <para>
    ///     The decorator adds a behavior to the pipeline requests,
    ///     it may modify the input value, perform some operation, 
    ///     stop or break the pipeline execution, or return a result value for request.
    /// </para>
    /// <para>
    ///     After the decorator behavior has been performed, the next handler should be called, 
    ///     otherwise execution will be interrupted.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">Data type of the request.</typeparam>
    /// <typeparam name="TResult">Result date type.</typeparam>
    public interface IDecorator<in TRequest, TResult>
    {
        /// <summary>
        /// Pipeline decorator handler.
        /// Perform any additional behavior and call <paramref name="next"/> delegate as necessary.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="next">
        ///     Delegate for the next action in the pipeline.
        /// </param>
        /// <returns>
        ///     The result of the request execution.
        /// </returns>
        TResult Handle(TRequest request, Func<TResult> next);
    }
}
