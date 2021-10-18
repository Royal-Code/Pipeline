using RoyalCode.PipelineFlow.Resolvers;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// <para>
    ///     The Pipeline Builder serves to setup the handlers of the pipelines.
    /// </para>
    /// <para>
    ///     For each type of input and output you can configure handlers and decorators.
    /// </para>
    /// </summary>
    public interface IPipelineBuilder
    {
        /// <summary>
        /// Adds an <see cref="IHandlerResolver"/>.
        /// </summary>
        /// <param name="resolver"></param>
        void AddHandlerResolver(IHandlerResolver resolver);

        /// <summary>
        /// Adds an <see cref="IDecoratorResolver"/>.
        /// </summary>
        /// <param name="resolver"></param>
        void AddDecoratorResolver(IDecoratorResolver resolver);

        /// <summary>
        /// Sets up a pipeline for a specific input type (<typeparamref name="TIn"/>).
        /// </summary>
        /// <typeparam name="TIn">The input type.</typeparam>
        /// <returns>A Pipeline Builder for the input type.</returns>
        IPipelineBuilder<TIn> Configure<TIn>();

        /// <summary>
        /// Sets up a pipeline for a specific input type (<typeparamref name="TIn"/>) and output type 
        /// (<typeparamref name="TOut"/>).
        /// </summary>
        /// <typeparam name="TIn">The input type.</typeparam>
        /// <typeparam name="TOut">The output type.</typeparam>
        /// <returns>A Pipeline Builder for the input and output types.</returns>
        IPipelineBuilder<TIn, TOut> Configure<TIn, TOut>();
    }

    /// <summary>
    /// <para>
    ///     The Pipeline Builder serves to setup the handlers of the pipelines.
    /// </para>
    /// <para>
    ///     This Pipeline Builder will setup handlers and decorators for the input type <typeparamref name="TIn"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    public interface IPipelineBuilder<TIn> : IPipelineBuilder
    {
        /// <summary>
        /// Configures a pipeline with dependency on a specific service (<typeparamref name="TService"/>).
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <returns>A Pipeline Builder for the input type with a service dependency.</returns>
        IPipelineBuilderWithService<TService, TIn> WithService<TService>();
    }

    /// <summary>
    /// <para>
    ///     The Pipeline Builder serves to setup the handlers of the pipelines.
    /// </para>
    /// <para>
    ///     This Pipeline Builder will setup handlers and decorators for the input type <typeparamref name="TIn"/>
    ///     and output type <typeparamref name="TOut"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    public interface IPipelineBuilder<TIn, TOut> : IPipelineBuilder
    {
        /// <summary>
        /// Configures a pipeline with dependency on a specific service (<typeparamref name="TService"/>).
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <returns>A Pipeline Builder for the input and output types with a service dependency..</returns>
        IPipelineBuilderWithService<TService, TIn, TOut> WithService<TService>();
    }
}
