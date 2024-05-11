using System.Security.Claims;

namespace ECommerce.Core.ServiceContracts.Users
{
    public interface IUserContextService
    {
        Guid? GetCustomerId(ClaimsIdentity? claimsIdentity);
    }
}
