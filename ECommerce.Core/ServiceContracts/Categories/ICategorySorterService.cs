using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Categories
{
    public interface ICategorySorterService
    {
        List<CategoryDto> Sort(IEnumerable<CategoryDto> categories, 
            SortOrder sortOrder = SortOrder.ASC);
    }
}
