using ECommerce.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Category
{
    public interface ICategoryUpdaterService
    {
        Task<bool> UpdateAsync(CategoryDto categoryDto);
    }
}
