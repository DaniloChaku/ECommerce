using ECommerce.Core.Dtos;
using ECommerce.Core.Helpers;
using ECommerce.Core.ServiceContracts.Manufacturers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing manufacturers in the admin area.
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class ManufacturersController : Controller
    {
        private readonly IManufacturerGetterService _manufacturerGetterService;
        private readonly IManufacturerAdderService _manufacturerAdderService;
        private readonly IManufacturerUpdaterService _manufacturerUpdaterService;
        private readonly IManufacturerDeleterService _manufacturerDeleterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturersController"/> class.
        /// </summary>
        /// <param name="manufacturerGetterService">The service for retrieving manufacturers.</param>
        /// <param name="manufacturerAdderService">The service for adding manufacturers.</param>
        /// <param name="manufacturerUpdaterService">The service for updating manufacturers.</param>
        /// <param name="manufacturerDeleterService">The service for deleting manufacturers.</param>
        public ManufacturersController(IManufacturerGetterService manufacturerGetterService,
            IManufacturerAdderService manufacturerAdderService, IManufacturerUpdaterService manufacturerUpdaterService,
            IManufacturerDeleterService manufacturerDeleterService)
        {
            _manufacturerGetterService = manufacturerGetterService;
            _manufacturerAdderService = manufacturerAdderService;
            _manufacturerUpdaterService = manufacturerUpdaterService;
            _manufacturerDeleterService = manufacturerDeleterService;
        }

        /// <summary>
        /// Displays the index view.
        /// </summary>
        /// <returns>The index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the view for adding or updating a manufacturer.
        /// </summary>
        /// <param name="id">The ID of the manufacturer to update, if any.</param>
        /// <returns>The view for adding or updating a manufacturer.</returns>
        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? id)
        {
            if (id is null || id == Guid.Empty)
            {
                return View(new ManufacturerDto());
            }

            var existingManufacturer = await _manufacturerGetterService.GetByIdAsync(id.Value);
            if (existingManufacturer is null)
            {
                existingManufacturer = new ManufacturerDto();
            }

            return View(existingManufacturer);
        }

        /// <summary>
        /// Handles the POST request for adding or updating a manufacturer.
        /// </summary>
        /// <param name="manufacturerDto">The manufacturer data to add or update.</param>
        /// <returns>The index view if successful, otherwise the add/update view with error messages.</returns>
        [HttpPost]
        public async Task<IActionResult> Upsert(ManufacturerDto manufacturerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(manufacturerDto);
            }

            try
            {
                if (manufacturerDto.Id == Guid.Empty)
                {
                    await _manufacturerAdderService.AddAsync(manufacturerDto);
                }
                else
                {
                    await _manufacturerUpdaterService.UpdateAsync(manufacturerDto);
                }

                TempData["success"] = $"Manufacturer {(manufacturerDto.Id == Guid.Empty
                    ? "created" : "updated")} successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(manufacturerDto), ex.Message);
                TempData["error"] = Constants.GENERIC_ERROR_MESSAGE;
                return View(manufacturerDto);
            }
        }

        #region API

        /// <summary>
        /// Checks if the manufacturer name is unique.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to check.</param>
        /// <returns>A JSON result indicating whether the manufacturer name is unique.</returns>
        [HttpGet]
        public async Task<IActionResult> IsManufacturerNameUnique(ManufacturerDto manufacturer)
        {
            if (manufacturer.Id != Guid.Empty)
            {
                var existingManufacturer = await _manufacturerGetterService.GetByIdAsync(manufacturer.Id);

                if (existingManufacturer is not null && existingManufacturer.Name == manufacturer.Name)
                {
                    return Json(true);
                }
            }

            var manufacturers = await _manufacturerGetterService.GetAllAsync();

            if (manufacturers.Any(t => t.Name == manufacturer.Name))
            {
                return Json(false);
            }

            return Json(true);
        }

        /// <summary>
        /// Retrieves all manufacturers.
        /// </summary>
        /// <returns>A JSON result containing all manufacturers.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ManufacturerDto> manufacturers = await _manufacturerGetterService.GetAllAsync();

                return Ok(new { data = manufacturers });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = Constants.GENERIC_ERROR_MESSAGE });
            }
        }

        /// <summary>
        /// Deletes a manufacturer.
        /// </summary>
        /// <param name="id">The ID of the manufacturer to delete.</param>
        /// <returns>An action result indicating the success or failure of the deletion operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var isDeleted = await _manufacturerDeleterService.DeleteAsync(id);

                if (!isDeleted)
                {
                    throw new InvalidOperationException();
                }

                var response = new
                {
                    Success = true,
                    Message = "Manufacturer deleted successfully."
                };
                TempData["success"] = response.Message;

                return Ok(response);
            }
            catch (Exception)
            {
                var response = new
                {
                    Success = false,
                    Message = "Failed to delete the manufacturer. Please try again later."
                };
                TempData["error"] = response.Message;

                return BadRequest(response);
            }
        }

        #endregion
    }
}
