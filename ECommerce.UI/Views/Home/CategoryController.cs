using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Category;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Views.Home
{
    public class CategoryController : Controller
    {
        private readonly ICategoryGetterService _categoryGetterService;

        public CategoryController(ICategoryGetterService categoryGetterService)
        {
            _categoryGetterService = categoryGetterService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<CategoryDto> categoryDtos = await _categoryGetterService.GetAllAsync();
            return Json(new { data = categoryDtos });
        }
    }
}
