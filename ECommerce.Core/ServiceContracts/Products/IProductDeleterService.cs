namespace ECommerce.Core.ServiceContracts.Products
{
    public interface IProductDeleterService
    {
        Task<bool> DeleteAsync(Guid id);
    }
}
