using ECommerce.Core.ServiceContracts.Users;
using System.Security.Claims;

namespace ECommerce.Core.Services.Users
{
    /// <summary>
    /// Service for accessing user context information.
    /// </summary>
    public class UserContextService : IUserContextService
    {
        /// <summary>
        /// Gets the customer ID from the provided claims identity.
        /// </summary>
        /// <param name="claimsIdentity">The claims identity containing user claims.</param>
        /// <returns>The customer ID if found in the claims identity; otherwise, null.</returns>
        public Guid? GetCustomerId(ClaimsIdentity? claimsIdentity)
        {
            var customerId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return customerId != null ? new Guid(customerId) : null;
        }
    }
}
