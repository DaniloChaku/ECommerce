using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Manufacturer;
using ECommerce.UI.Views.Home;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerGetterService _manufacturerGetterService;
        private readonly IManufacturerAdderService _manufacturerAdderService;
        private readonly IManufacturerUpdaterService _manufacturerUpdaterService;
        private readonly IManufacturerDeleterService _manufacturerDeleterService;

        public ManufacturerController(IManufacturerGetterService manufacturerGetterService,
            IManufacturerAdderService manufacturerAdderService, IManufacturerUpdaterService manufacturerUpdaterService,
            IManufacturerDeleterService manufacturerDeleterService)
        {
            _manufacturerGetterService = manufacturerGetterService;
            _manufacturerAdderService = manufacturerAdderService;
            _manufacturerUpdaterService = manufacturerUpdaterService;
            _manufacturerDeleterService = manufacturerDeleterService;
        }

        public IActionResult Index()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ManufacturerDto manufacturerDto)
        {
            throw new NotImplementedException();
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
