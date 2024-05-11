using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing products.
    /// </summary>
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The updated product data.</param>
        /// <returns>The updated product.</returns>
        public async Task<Product> UpdateAsync(Product product)
        {
            var existingProduct = await _dbSet.FindAsync(product.Id);

            existingProduct!.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.SalePrice = product.SalePrice;
            existingProduct.PriceType = product.PriceType;
            existingProduct.Stock = product.Stock;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.ManufacturerId = product.ManufacturerId;
            existingProduct.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return existingProduct;
        }

        /// <summary>
        /// Retrieves all products optionally filtered by a predicate.
        /// </summary>
        /// <param name="filter">An optional predicate to filter the products.</param>
        /// <returns>A list of products matching the filter predicate, if provided; otherwise, all products.</returns>
        public override async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>>? filter = null)
        {
            IQueryable<Product> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.Include(nameof(Category)).Include(nameof(Manufacturer));

            return await query.ToListAsync();
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        public async override Task<Product?> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include("Category").Include("Manufacturer").FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
