using System.Linq.Expressions;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
    }
}
