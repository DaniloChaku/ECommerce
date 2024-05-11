using System.Security.Claims;

namespace ECommerce.Core.ServiceContracts.Users
{
    /// <summary>
    /// Defines the contract for accessing user context information.
    /// </summary>
    public interface IUserContextService
    {
        /// <summary>
        /// Retrieves the customer identifier from the provided claims identity.
        /// </summary>
        /// <param name="claimsIdentity">The claims identity of the user.</param>
        /// <returns>
        /// The customer identifier if found in the claims identity; otherwise, null.
        /// </returns>
        Guid? GetCustomerId(ClaimsIdentity? claimsIdentity);
    }
}
