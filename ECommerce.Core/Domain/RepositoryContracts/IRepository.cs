using System.Linq.Expressions;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a generic repository interface for accessing data.
    /// </summary>
    /// <typeparam name="TEntity">The type of entities managed by the repository.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Retrieves all entities asynchronously from the database.
        /// </summary>
        /// <param name="filter">An optional filter to apply to the entities.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of entities.</returns>
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);

        /// <summary>
        /// Retrieves an entity by its unique identifier asynchronously from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the retrieved entity or null if not found.</returns>
        Task<TEntity?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds an entity to the database asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add to the database.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the added entity.</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity from the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete from the repository.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains true if the entity was successfully deleted; otherwise, false.</returns>
        Task<bool> DeleteAsync(TEntity entity);
    }
}
