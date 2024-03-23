using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var existingProduct = await _dbSet.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.SalePrice = product.SalePrice;
            existingProduct.Stock = product.Stock;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.ManufacturerId = product.ManufacturerId;
            existingProduct.CategoryId = product.CategoryId;

            return await SaveAsync();
        }

        public override async Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>>? filter = null)
        {
            IQueryable<Product> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query.Include(nameof(Category)).Include(nameof(Manufacturer));

            return await query.ToListAsync();
        }
    }
}
