﻿using ECommerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> UpdateAsync(Product product);
    }
}