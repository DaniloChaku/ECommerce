﻿using ECommerce.Core.Domain.Entities;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    public interface IManufacturerRepository : IRepository<Manufacturer>
    {
        Task<Manufacturer> UpdateAsync(Manufacturer manufacturer);
    }
}
