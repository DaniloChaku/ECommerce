using ECommerce.Core.ServiceContracts.Users;
using System.Security.Claims;

namespace ECommerce.Core.Services.Users
{
    public class UserContextService : IUserContextService
    {
        public Guid? GetCustomerId(ClaimsIdentity? claimsIdentity)
        {
            var customerId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return customerId != null ? new Guid(customerId) : null;
        }
    }
}
