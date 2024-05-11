using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Domain.IdentityEntities
{
    /// <summary>
    /// Represents an application user in the system.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; } = "Unknown";

        /// <summary>
        /// Gets or sets the address of the user.
        /// </summary>
        public string? Address { get; set; }
    }
}
