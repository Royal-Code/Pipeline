using System;

namespace RoyalCode.CommandAndQuery.OperationsCommands
{
    /// <summary>
    /// A command to update an existing entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be updated.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TResult">The operation result type.</typeparam>
    public class UpdateCommand<TEntity, TId, TResult> : IRequest<TResult>
        where TEntity : class
    {
        /// <summary>
        /// The entity to be updated.
        /// </summary>
        public TEntity Entity { get; }

        /// <summary>
        /// The entity id.
        /// </summary>
        public TId Id { get; }

        /// <summary>
        /// Create a new command with the entity to be updated.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <param name="id">The entity id.</param>
        public UpdateCommand(TEntity entity, TId id)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            Id = id;
        }
    }
}
