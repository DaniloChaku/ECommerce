namespace ECommerce.Core.ServiceContracts.Categories
{
    public interface ICategoryDeleterService
    {
        Task<bool> DeleteAsync(Guid id);
    }
}
