namespace ECommerce.Core.ServiceContracts.Images
{
    /// <summary>
    /// Defines the contract for deleting images.
    /// </summary>
    public interface IImageDeleterService
    {
        /// <summary>
        /// Deletes the image specified by the URL.
        /// </summary>
        /// <param name="imageUrl">The URL of the image to delete.</param>
        void DeleteImage(string? imageUrl);

        /// <summary>
        /// Deletes the image folder associated with the specified product.
        /// </summary>
        /// <param name="productId">The identifier of the product whose image folder should be deleted.</param>
        void DeleteImageFolder(string productId);
    }
}
