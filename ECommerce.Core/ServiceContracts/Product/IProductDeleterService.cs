﻿using ECommerce.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Product
{
    public interface IProductDeleterService
    {
        Task<ResultDto<ManufacturerDto>> DeleteAsync(Guid id);
    }
}
