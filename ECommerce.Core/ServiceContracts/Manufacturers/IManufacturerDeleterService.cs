namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    public interface IManufacturerDeleterService
    {
        Task<bool> DeleteAsync(Guid id);
    }
}
