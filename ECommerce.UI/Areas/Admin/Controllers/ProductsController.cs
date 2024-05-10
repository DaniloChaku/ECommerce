using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Helpers;
using ECommerce.Core.ServiceContracts.Category;
using ECommerce.Core.ServiceContracts.Image;
using ECommerce.Core.ServiceContracts.Manufacturer;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.Core.Settings;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class ProductsController : Controller
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

        private readonly IImageUploaderService _imageUploaderService;
        private readonly IImageDeleterService _imageDeleterService;
        private readonly ImageUploadOptions _imageUploadOptions;

        public ProductsController(IProductGetterService productGetterService,
            IProductAdderService productAdderService, IProductUpdaterService productUpdaterService,
            IProductDeleterService productDeleterService, ICategoryGetterService categoryGetterService,
            ICategorySorterService categorySorterService, IManufacturerGetterService manufacturerGetterService,
            IManufacturerSorterService manufacturerSorterService, IWebHostEnvironment webHostEnvironment,
            IImageUploaderService imageUploaderService, IImageDeleterService imageDeleterService,
            IOptions<ImageUploadOptions> imageUploadOptions)
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

            _imageUploaderService = imageUploaderService;
            _imageDeleterService = imageDeleterService;
            _imageUploadOptions = imageUploadOptions.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? id)
        {
            var productUpsertModel = new ProductUpsertModel()
            {
                Categories = await GetCategoriesSelectList(),
                Manufacturers = await GetManufacturersSelectList(),
                ImageUploadOptions = _imageUploadOptions
            };

            if (id is not null && id != Guid.Empty)
            {
                productUpsertModel.Product = await _productGetterService.GetByIdAsync(id.Value)
                ?? new ProductDto();
            }

            return View(productUpsertModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductUpsertModel productModel)
        {
            if (!productModel.Categories.Any())
            {
                productModel.Categories = await GetCategoriesSelectList();
            }
            if (!productModel.Manufacturers.Any())
            {
                productModel.Manufacturers = await GetManufacturersSelectList();
            }
            productModel.ImageUploadOptions ??= _imageUploadOptions;

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

                if (productModel.Image is not null)
                {
                    try
                    {
                        var imageUrl = await _imageUploaderService.UploadAsync(productModel.Image,
                            productResponse.Id.ToString());

                        _imageDeleterService.DeleteImage(productResponse.ImageUrl);

                        productResponse.ImageUrl = imageUrl;

                        await _productUpdaterService.UpdateAsync(productResponse);
                    }
                    catch (ImageUploadException ex)
                    {
                        ModelState.AddModelError(nameof(productModel.Image), ex.Message);
                        TempData["error"] = ex.Message;
                        return View(productModel);
                    }
                    catch (Exception)
                    {
                        TempData["error"] = "Failed to add the image";
                        return View(productModel);
                    }
                }

                TempData["success"] = $"Product {(productModel.Product.Id == Guid.Empty
                    ? "created" : "updated")} successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while processing your request. Please try again.";
                return View(productModel);
            }
        }

        #region API

        public async Task<IActionResult> IsProductNameUnique(ProductDto product)
        {
            if (product.Id != Guid.Empty)
            {
                var existingCategory = await _productGetterService.GetByIdAsync(product.Id);

                if (existingCategory!.Name == product.Name)
                {
                    return Json(true);
                }
            }

            var categorys = await _productGetterService.GetAllAsync();

            if (categorys.Any(t => t.Name == product.Name))
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
                var product = await _productGetterService.GetByIdAsync(id);

                if (product is null)
                {
                    return BadRequest(
                        new
                        {
                            Success = false,
                            Message = "Product not found."
                        });
                }

                var isDeleted = await _productDeleterService.DeleteAsync(id);

                if (!isDeleted)
                {
                    throw new InvalidOperationException("Failed to delete the product. Please try again later.");
                }

                try
                {
                    if (product.ImageUrl != null)
                    {
                        _imageDeleterService.DeleteImageFolder(id.ToString());
                    }
                }
                catch (Exception) { }

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
                    Message = "An error occurred while deleting the product. Please try again later."
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> HasReference(string type, Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Id cannot be null"
                });
            }

            try
            {
                var products = type.ToLower() switch
                {
                    "category" => await _productGetterService.GetByCategoryAsync(id),
                    "manufacturer" => await _productGetterService.GetByManufacturerAsync(id),
                    _ => throw new ArgumentException("The referenced type is invalid")
                };

                if (products.Count is 0)
                {
                    return Ok(new { success = true, hasAssociations = false });
                }

                return Ok(new { success = true, hasAssociations = true });
            }
            catch (ArgumentException ex)
            {
                var response = new
                {
                    success = false,
                    message = ex.Message
                };

                return BadRequest(response);
            }
            catch (Exception)
            {
                var response = new
                {
                    success = false,
                    Message = "An error occurred. Please try again later."
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveReference(string type, Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Id cannot be null"
                });
            }

            try
            {
                var products = new List<ProductDto>();
                switch (type.ToLower())
                {
                    case "category":
                        products = await _productGetterService.GetByCategoryAsync(id);

                        foreach (var product in products)
                        {
                            product.CategoryId = null;
                            await _productUpdaterService.UpdateAsync(product);
                        }
                        break;
                    case "manufacturer":
                        products = await _productGetterService.GetByManufacturerAsync(id);

                        foreach (var product in products)
                        {
                            product.ManufacturerId = null;
                            await _productUpdaterService.UpdateAsync(product);
                        }
                        break;
                    default:
                        throw new ArgumentException("The referenced type is invalid");
                }

                return Ok(new { success = true });
            }
            catch (ArgumentException ex)
            {
                var response = new
                {
                    success = false,
                    message = ex.Message
                };

                return BadRequest(response);
            }
            catch (Exception)
            {
                var response = new
                {
                    success = false,
                    Message = "An error occurred. Please try again later."
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        #endregion

        private async Task<IEnumerable<SelectListItem>> GetCategoriesSelectList()
        {
            return _categorySorterService.Sort(await
                _categoryGetterService.GetAllAsync()).Select(t =>
                new SelectListItem() { Text = t.Name, Value = t.Id.ToString() });
        }

        private async Task<IEnumerable<SelectListItem>> GetManufacturersSelectList()
        {
            return _manufacturerSorterService.Sort(await
                _manufacturerGetterService.GetAllAsync()).Select(t =>
                new SelectListItem() { Text = t.Name, Value = t.Id.ToString() });
        }

        private IEnumerable<SelectListItem> GetPriceTypesSelectList()
        {
            return Enum.GetNames(typeof(PriceType)).Select(
                t => new SelectListItem() { Text = t, Value = t });
        }
    }
}
