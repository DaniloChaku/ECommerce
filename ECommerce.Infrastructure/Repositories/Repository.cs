using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Repositories
{
    /// <summary>
    /// Abstract base class for repository implementations.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Retrieves all entities optionally filtered by a predicate.
        /// </summary>
        /// <param name="filter">An optional predicate to filter the entities.</param>
        /// <returns>A list of entities matching the filter predicate, if provided; otherwise, all entities.</returns>
        public virtual async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity with the specified ID, or null if not found.</returns>
        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>True if the entity was deleted successfully; otherwise, false.</returns>
        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);

            var deletedRows = await _context.SaveChangesAsync();

            return deletedRows == 1;
        }
    }
}
