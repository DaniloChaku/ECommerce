using ECommerce.Core.Domain.Entities;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> UpdateAsync(Product product);
    }
}
