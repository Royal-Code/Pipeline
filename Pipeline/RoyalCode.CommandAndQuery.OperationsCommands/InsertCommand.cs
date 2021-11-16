using System;

namespace RoyalCode.CommandAndQuery.OperationsCommands
{
    /// <summary>
    /// A command to insert a new entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be inserted.</typeparam>
    /// <typeparam name="TResult">The operation result type.</typeparam>
    public class InsertCommand<TEntity, TResult> : IRequest<TResult>
        where TEntity : class
    {
        /// <summary>
        /// The entity to be inserted.
        /// </summary>
        public TEntity Entity { get; }

        /// <summary>
        /// Create a new command with the entity to be inserted.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        public InsertCommand(TEntity entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }
    }
}
