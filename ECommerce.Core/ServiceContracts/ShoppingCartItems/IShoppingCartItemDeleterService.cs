namespace ECommerce.Core.ServiceContracts.ShoppingCartItems
{
    public interface IShoppingCartItemDeleterService
    {
        Task<bool> DeleteAsync(Guid id);
    }
}
