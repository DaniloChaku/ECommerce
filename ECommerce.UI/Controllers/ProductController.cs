using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Category;
using ECommerce.Core.ServiceContracts.Manufacturer;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductGetterService _productGetterService;
        private readonly IProductAdderService _productAdderService;
        private readonly IProductUpdaterService _productUpdaterService;
        private readonly IProductDeleterService _productDeleterService;

        private readonly ICategoryGetterService _categoryGetterService;
        private readonly ICategorySorterService _categorySorterService;
        private readonly IManufacturerGetterService _manufacturerGetterService;
        private readonly IManufacturerSorterService _manufacturerSorterService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductGetterService productGetterService,
            IProductAdderService productAdderService, IProductUpdaterService productUpdaterService,
            IProductDeleterService productDeleterService, ICategoryGetterService categoryGetterService,
            ICategorySorterService categorySorterService, IManufacturerGetterService manufacturerGetterService,
            IManufacturerSorterService manufacturerSorterService, IWebHostEnvironment webHostEnvironment)
        {
            _productGetterService = productGetterService;
            _productAdderService = productAdderService;
            _productUpdaterService = productUpdaterService;
            _productDeleterService = productDeleterService;

            _categoryGetterService = categoryGetterService;
            _categorySorterService = categorySorterService;
            _manufacturerGetterService = manufacturerGetterService;
            _manufacturerSorterService = manufacturerSorterService;

            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? id)
        {
            var categoriesSelectList = _categorySorterService.Sort(await
                _categoryGetterService.GetAllAsync()).Select(t =>
                new SelectListItem() { Text = t.Name, Value = t.Id.ToString() });

            var manufacturersSelectList = _manufacturerSorterService.Sort(await
                _manufacturerGetterService.GetAllAsync()).Select(t =>
                new SelectListItem() { Text = t.Name, Value = t.Id.ToString() });

            var productUpsertModel = new ProductUpsertModel()
            {
                Categories = categoriesSelectList,
                Manufacturers = manufacturersSelectList
            };

            if (id is not null && id != Guid.Empty)
            {
                productUpsertModel.Product = await _productGetterService.GetByIdAsync(id.Value)
                ?? new ProductDto();
            }

            return View(productUpsertModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductUpsertModel productModel, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View(productModel);
            }

            try
            {
                ProductDto productResponse;

                if (productModel.Product.Id == Guid.Empty)
                {
                    productResponse = await _productAdderService.AddAsync(productModel.Product);
                }
                else
                {
                    productResponse = await _productUpdaterService.UpdateAsync(productModel.Product);
                }

                if (image is not null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string productPath = @"images/products/product-" + productResponse.Id;
                    string finalPath = Path.Combine(wwwrootPath, productPath);

                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    if (productResponse.ImageUrl is not null)
                    {
                        string existingImagePath = Path.Combine(wwwrootPath, productResponse.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(existingImagePath))
                            System.IO.File.Delete(existingImagePath);
                    }

                    productResponse.ImageUrl = $"/{productPath}/{fileName}";

                    await _productUpdaterService.UpdateAsync(productResponse);
                }

                TempData["success"] = $"Product {(productModel.Product.Id == Guid.Empty
                    ? "created" : "updated")} successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(productModel), ex.Message);
                TempData["error"] = "An error occurred while processing your request. Please try again.";
                return View(productModel);
            }
        }

        #region API

        public async Task<IActionResult> ValidateSameName(ProductDto product)
        {
            if (product.Id != Guid.Empty)
            {
                var products = await _productGetterService.GetAllAsync();

                if (products.Any(t => t.Name == product.Name))
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
                List<ProductDto> products = await _productGetterService.GetAllAsync();

                return Ok(new { data = products });
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
                var isDeleted = await _productDeleterService.DeleteAsync(id);

                if (!isDeleted)
                {
                    throw new InvalidOperationException("Failed to delete Product.");
                }

                var response = new
                {
                    Success = true,
                    Message = "Product deleted successfully."
                };

                return Ok(response);
            }
            catch (Exception)
            {
                var response = new
                {
                    success = false,
                    Message = "An error occurred while deleting Product. Please try again."
                };

                return BadRequest(response);
            }
        }

        #endregion
    }
}
