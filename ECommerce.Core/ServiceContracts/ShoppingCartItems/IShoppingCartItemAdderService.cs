using ECommerce.Core.Dtos;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.ShoppingCartItems
{
    /// <summary>
    /// Defines the contract for adding a shopping cart item.
    /// </summary>
    public interface IShoppingCartItemAdderService
    {
        /// <summary>
        /// Adds a new shopping cart item asynchronously.
        /// </summary>
        /// <param name="shoppingCartItemDto">The data transfer object representing the shopping cart item to add.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the added shopping cart item.</returns>
        Task<ShoppingCartItemDto> AddAsync(ShoppingCartItemDto shoppingCartItemDto);
    }
}
