using ECommerce.Core.Dtos;
using ECommerce.Core.Helpers;
using ECommerce.Core.ServiceContracts.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class CategoryController : Controller
    {
        private readonly ICategoryGetterService _categoryGetterService;
        private readonly ICategoryAdderService _categoryAdderService;
        private readonly ICategoryUpdaterService _categoryUpdaterService;
        private readonly ICategoryDeleterService _categoryDeleterService;

        public CategoryController(ICategoryGetterService categoryGetterService,
            ICategoryAdderService categoryAdderService, ICategoryUpdaterService categoryUpdaterService,
            ICategoryDeleterService categoryDeleterService)
        {
            _categoryGetterService = categoryGetterService;
            _categoryAdderService = categoryAdderService;
            _categoryUpdaterService = categoryUpdaterService;
            _categoryDeleterService = categoryDeleterService;
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
                return View(new CategoryDto());
            }

            var existingCategory = await _categoryGetterService.GetByIdAsync(id.Value);
            if (existingCategory is null)
            {
                existingCategory = new CategoryDto();
            }

            return View(existingCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }

            try
            {
                if (categoryDto.Id == Guid.Empty)
                {
                    await _categoryAdderService.AddAsync(categoryDto);
                }
                else
                {
                    await _categoryUpdaterService.UpdateAsync(categoryDto);
                }

                TempData["success"] = $"Category {(categoryDto.Id == Guid.Empty
                    ? "created" : "updated")} successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while processing your request. Please try again later.";
                return View(categoryDto);
            }
        }

        #region API

        public async Task<IActionResult> IsCategoryNameUnique(CategoryDto category)
        {
            if (category.Id != Guid.Empty)
            {
                var existingCategory = await _categoryGetterService.GetByIdAsync(category.Id);

                if (existingCategory!.Name == category.Name)
                {
                    return Json(true);
                }
            }

            var categorys = await _categoryGetterService.GetAllAsync();

            if (categorys.Any(t => t.Name == category.Name))
            {
                return Json(false);
            }

            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<CategoryDto> categories = await _categoryGetterService.GetAllAsync();

                return Ok(new { data = categories });
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
                var isDeleted = await _categoryDeleterService.DeleteAsync(id);

                if (!isDeleted)
                {
                    throw new InvalidOperationException("Failed to delete the category.");
                }

                var response = new
                {
                    Success = true,
                    Message = "Category deleted successfully."
                };

                return Ok(response);
            }
            catch (Exception)
            {
                var response = new
                {
                    success = false,
                    Message = "An error occurred while deleting the category. Please try again later."
                };

                return BadRequest(response);
            }
        }

        #endregion
    }
}
