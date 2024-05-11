using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing shopping cart items.
    /// </summary>
    public class ShoppingCartItemRepository : Repository<ShoppingCartItem>, IShoppingCartItemRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public ShoppingCartItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Updates an existing shopping cart item.
        /// </summary>
        /// <param name="shoppingCartItem">The updated shopping cart item data.</param>
        /// <returns>The updated shopping cart item.</returns>
        public async Task<ShoppingCartItem> UpdateAsync(ShoppingCartItem shoppingCartItem)
        {
            var existingShoppingCartItem = await _dbSet.FirstOrDefaultAsync(i => i.Id == shoppingCartItem.Id);

            existingShoppingCartItem!.Count = shoppingCartItem.Count;
            existingShoppingCartItem.CustomerId = shoppingCartItem.CustomerId;
            existingShoppingCartItem.ProductId = shoppingCartItem.ProductId;

            await _context.SaveChangesAsync();

            return existingShoppingCartItem;
        }

        /// <summary>
        /// Retrieves all shopping cart items optionally filtered by a predicate.
        /// </summary>
        /// <param name="filter">An optional predicate to filter the shopping cart items.</param>
        /// <returns>A list of shopping cart items matching the filter predicate, if provided; otherwise, all shopping cart items.</returns>
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

        /// <summary>
        /// Retrieves a shopping cart item by its ID.
        /// </summary>
        /// <param name="id">The ID of the shopping cart item to retrieve.</param>
        /// <returns>The shopping cart item with the specified ID, or null if not found.</returns>
        public async override Task<ShoppingCartItem?> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(nameof(Product)).FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
