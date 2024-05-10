using ECommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Categories
{
    public interface ICategoryDeleterService
    {
        Task<bool> DeleteAsync(Guid id);
    }
}
