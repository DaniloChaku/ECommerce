﻿using ECommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    public interface IManufacturerAdderService
    {
        Task<ManufacturerDto> AddAsync(ManufacturerDto manufacturerDto); 
    }
}