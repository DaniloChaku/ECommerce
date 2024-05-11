using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; } = "Unknown";
        public string? Address { get; set; }
    }
}
