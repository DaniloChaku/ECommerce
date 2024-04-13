using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Infrastructure.Db;
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
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var existingProduct = await _dbSet.FindAsync(product.Id);

            existingProduct!.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.SalePrice = product.SalePrice;
            existingProduct.Stock = product.Stock;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.ManufacturerId = product.ManufacturerId;
            existingProduct.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return existingProduct;
        }

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

        public async override Task<Product?> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include("Category").Include("Manufacturer").FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
