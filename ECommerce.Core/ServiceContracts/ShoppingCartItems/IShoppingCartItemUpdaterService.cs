using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.ShoppingCartItems
{
    /// <summary>
    /// Defines the contract for updating a shopping cart item.
    /// </summary>
    public interface IShoppingCartItemUpdaterService
    {
        /// <summary>
        /// Updates a shopping cart item asynchronously.
        /// </summary>
        /// <param name="shoppingCartItemDto">The data transfer object representing the shopping cart item to update.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated shopping cart item.</returns>
        Task<ShoppingCartItemDto> UpdateAsync(ShoppingCartItemDto shoppingCartItemDto);
    }
}
