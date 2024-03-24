using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Product
{
    public interface IProductDeleterService
    {
        Task<bool> DeleteAsync(Guid id);
    }
}
