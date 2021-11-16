using System;

namespace RoyalCode.CommandAndQuery.OperationsCommands
{
    /// <summary>
    /// A generic command for creating an entity from a data model (DTO).
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be created.</typeparam>
    /// <typeparam name="TDto">The data model type that contains the entity data.</typeparam>
    /// <typeparam name="TResult">The operation result type.</typeparam>
    public class CreateCommand<TEntity, TDto, TResult> : IRequest<TResult>
        where TEntity : class
        where TDto : class
    {
        /// <summary>
        /// The data model.
        /// </summary>
        public TDto Model { get; }

        /// <summary>
        /// Create a new command with the data model to create a new entity.
        /// </summary>
        /// <param name="model">The data model.</param>
        public CreateCommand(TDto model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
    }
}
