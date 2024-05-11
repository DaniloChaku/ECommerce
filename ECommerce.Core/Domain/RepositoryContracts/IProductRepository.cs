using ECommerce.Core.Domain.Entities;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository interface for managing products.
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Updates a product asynchronously in the database.
        /// </summary>
        /// <param name="product">The product to update.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated product.</returns>
        Task<Product> UpdateAsync(Product product);
    }
}
