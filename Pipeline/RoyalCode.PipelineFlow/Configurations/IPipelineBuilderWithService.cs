namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// <para>
    ///     The Pipeline Builder serves to setup the handlers of the pipelines.
    /// </para>
    /// <para>
    ///     This Pipeline Builder sets up handlers with dependencies on a specific service.
    /// </para>
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    public interface IPipelineBuilderWithService<TService> : IPipelineBuilder { }

    /// <summary>
    /// <para>
    ///     The Pipeline Builder serves to setup the handlers of the pipelines.
    /// </para>
    /// <para>
    ///     This Pipeline Builder sets up handlers with dependencies on a specific service.
    /// </para>
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <typeparam name="TIn">The input type.</typeparam>
    public interface IPipelineBuilderWithService<TService, TIn> : IPipelineBuilderWithService<TService> { }

    /// <summary>
    /// <para>
    ///     The Pipeline Builder serves to setup the handlers of the pipelines.
    /// </para>
    /// <para>
    ///     This Pipeline Builder sets up handlers with dependencies on a specific service.
    /// </para>
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    public interface IPipelineBuilderWithService<TService, TIn, TOut> : IPipelineBuilderWithService<TService> { }
}
