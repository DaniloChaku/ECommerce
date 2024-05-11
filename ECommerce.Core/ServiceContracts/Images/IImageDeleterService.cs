namespace ECommerce.Core.ServiceContracts.Images
{
    public interface IImageDeleterService
    {
        void DeleteImage(string? imageUrl);
        void DeleteImageFolder(string productId);
    }
}
