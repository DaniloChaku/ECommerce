using ECommerce.Core.Dtos;
using ECommerce.Core.Helpers;
using ECommerce.Core.ServiceContracts.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing categories in the admin area.
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class CategoriesController : Controller
    {
        private readonly ICategoryGetterService _categoryGetterService;
        private readonly ICategoryAdderService _categoryAdderService;
        private readonly ICategoryUpdaterService _categoryUpdaterService;
        private readonly ICategoryDeleterService _categoryDeleterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="categoryGetterService">The service for retrieving categories.</param>
        /// <param name="categoryAdderService">The service for adding categories.</param>
        /// <param name="categoryUpdaterService">The service for updating categories.</param>
        /// <param name="categoryDeleterService">The service for deleting categories.</param>
        public CategoriesController(ICategoryGetterService categoryGetterService,
            ICategoryAdderService categoryAdderService, ICategoryUpdaterService categoryUpdaterService,
            ICategoryDeleterService categoryDeleterService)
        {
            _categoryGetterService = categoryGetterService;
            _categoryAdderService = categoryAdderService;
            _categoryUpdaterService = categoryUpdaterService;
            _categoryDeleterService = categoryDeleterService;
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
        /// Displays the view for adding or updating a category.
        /// </summary>
        /// <param name="id">The ID of the category to update, if any.</param>
        /// <returns>The view for adding or updating a category.</returns>
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

        /// <summary>
        /// Handles the POST request for adding or updating a category.
        /// </summary>
        /// <param name="categoryDto">The category data to add or update.</param>
        /// <returns>The index view if successful, otherwise the add/update view with error messages.</returns>
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
                TempData["error"] = Constants.GENERIC_ERROR_MESSAGE;
                return View(categoryDto);
            }
        }

        #region API

        /// <summary>
        /// Checks if the category name is unique.
        /// </summary>
        /// <param name="category">The category to check.</param>
        /// <returns>A JSON result indicating whether the category name is unique.</returns>
        [HttpGet]
        public async Task<IActionResult> IsCategoryNameUnique(CategoryDto category)
        {
            if (category.Id != Guid.Empty)
            {
                var existingCategory = await _categoryGetterService.GetByIdAsync(category.Id);

                if (existingCategory is not null && existingCategory.Name == category.Name)
                {
                    return Json(true);
                }
            }

            var categories = await _categoryGetterService.GetAllAsync();

            if (categories.Any(t => t.Name == category.Name))
            {
                return Json(false);
            }

            return Json(true);
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A JSON result containing all categories.</returns>
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
                    new { error = Constants.GENERIC_ERROR_MESSAGE });
            }
        }

        /// <summary>
        /// Deletes a category.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>An action result indicating the success or failure of the deletion operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var isDeleted = await _categoryDeleterService.DeleteAsync(id);

                if (!isDeleted)
                {
                    throw new InvalidOperationException();
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
                    Message = "Failed to delete the category. Please try again later."
                };

                return BadRequest(response);
            }
        }

        #endregion
    }
}
