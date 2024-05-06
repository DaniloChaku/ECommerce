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
    public class ShoppingCartItemRepository : Repository<ShoppingCartItem>, IShoppingCartItemRepository
    {
        public ShoppingCartItemRepository(ApplicationDbContext context) : base(context) 
        { 
        }

        public async Task<ShoppingCartItem> UpdateAsync(ShoppingCartItem shoppingCartItem)
        {
            var existingShoppingCartItem = await _dbSet.FirstOrDefaultAsync(i => i.Id == shoppingCartItem.Id);

            existingShoppingCartItem!.Count = shoppingCartItem.Count;
            existingShoppingCartItem.CustomerId = shoppingCartItem.CustomerId;
            existingShoppingCartItem.ProductId = shoppingCartItem.ProductId;

            await _context.SaveChangesAsync();

            return existingShoppingCartItem;
        }

        public override async Task<List<ShoppingCartItem>> GetAllAsync(Expression<Func<ShoppingCartItem, bool>>? filter = null)
        {
            IQueryable<ShoppingCartItem> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.Include(nameof(Product));

            return await query.ToListAsync();
        }

        public async override Task<ShoppingCartItem?> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(nameof(Product)).FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
