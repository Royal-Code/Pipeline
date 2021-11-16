using System;

namespace RoyalCode.CommandAndQuery.OperationsCommands
{
    /// <summary>
    /// <para>
    ///     A generic command to modify an existing entity from a data model (DTO).
    /// </para>
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be modified.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TDto">The data model type that contains the entity data.</typeparam>
    /// <typeparam name="TResult">The operation result type.</typeparam>
    public class ModifyCommand<TEntity, TId, TDto, TResult> : IRequest<TResult>
        where TEntity : class
        where TDto : class
    {
        /// <summary>
        /// The data model.
        /// </summary>
        public TDto Model { get; }

        /// <summary>
        /// The entity id.
        /// </summary>
        public TId Id { get; }

        /// <summary>
        /// Create a new command with the data model to update an existing entity.
        /// </summary>
        /// <param name="model">The data model.</param>
        /// <param name="id">The entity id.</param>
        public ModifyCommand(TDto model, TId id)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Id = id;
        }
    }
}
