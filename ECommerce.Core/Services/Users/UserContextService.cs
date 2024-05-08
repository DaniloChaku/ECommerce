using ECommerce.Core.ServiceContracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
