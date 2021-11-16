using System;

namespace RoyalCode.CommandAndQuery.OperationsCommands
{
    /// <summary>
    /// Command to delete an entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be created.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TResult">The operation result type.</typeparam>
    public class DeleteCommand<TEntity, TId, TResult> : IRequest<TResult>
        where TEntity : class
    {
        /// <summary>
        /// Id da entidade.
        /// </summary>
        public TId Id { get; }

        /// <summary>
        /// Create a new command with the identification of the entity that will be deleted.
        /// </summary>
        /// <param name="id">The entity id.</param>
        public DeleteCommand(TId id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }
}
