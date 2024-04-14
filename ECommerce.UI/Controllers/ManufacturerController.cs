﻿using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Manufacturer;
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
            return View();
        }

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
                return RedirectToAction(nameof(ManufacturerController.Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(manufacturerDto), ex.Message);
                TempData["error"] = "An error occurred while processing your request. Please try again.";
                return View(manufacturerDto);
            }
        }

        #region API

        public async Task<IActionResult> ValidateSameName(ManufacturerDto manufacturer)
        {
            if (manufacturer.Id != Guid.Empty)
            {
                var manufacturers = await _manufacturerGetterService.GetAllAsync();

                if (manufacturers.Any(t => t.Name == manufacturer.Name))
                {
                    return Json(false);
                }
            }

            return Json(true);
        }

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
                    new { error = "An error occurred while processing your request. Please try again later." });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var isDeleted = await _manufacturerDeleterService.DeleteAsync(id);

                if (!isDeleted)
                {
                    throw new InvalidOperationException("Failed to delete Manufacturer.");
                }

                var response = new
                {
                    Success = true,
                    Message = "Manufacturer deleted successfully."
                };
                TempData["success"] = response.Message;

                return Ok(response);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(id), ex.Message);

                var response = new
                {
                    Success = false,
                    Message = "An error occurred while deleting Manufacturer. Please try again."
                };
                TempData["error"] = response.Message;

                return BadRequest(response);
            }
        }

        #endregion
    }
}
