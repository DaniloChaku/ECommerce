using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Helpers;
using ECommerce.Core.ServiceContracts.Categories;
using ECommerce.Core.ServiceContracts.Images;
using ECommerce.Core.ServiceContracts.Manufacturers;
using ECommerce.Core.ServiceContracts.Products;
using ECommerce.Core.Settings;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing products in the admin area.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="productGetterService">The service for retrieving products.</param>
        /// <param name="productAdderService">The service for adding products.</param>
        /// <param name="productUpdaterService">The service for updating products.</param>
        /// <param name="productDeleterService">The service for deleting products.</param>
        /// <param name="categoryGetterService">The service for retrieving categories.</param>
        /// <param name="categorySorterService">The service for sorting categories.</param>
        /// <param name="manufacturerGetterService">The service for retrieving manufacturers.</param>
        /// <param name="manufacturerSorterService">The service for sorting manufacturers.</param>
        /// <param name="webHostEnvironment">The hosting environment.</param>
        /// <param name="imageUploaderService">The service for uploading images.</param>
        /// <param name="imageDeleterService">The service for deleting images.</param>
        /// <param name="imageUploadOptions">The image upload options.</param>
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

        /// <summary>
        /// Displays the index view.
        /// </summary>
        /// <returns>The index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the view for adding or updating a product.
        /// </summary>
        /// <param name="id">The ID of the product to update, if any.</param>
        /// <returns>The view for adding or updating a product.</returns>
        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? id)
        {
            var productUpsertModel = new ProductUpsertViewModel()
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

        /// <summary>
        /// Handles the POST request for adding or updating a product.
        /// </summary>
        /// <param name="productModel">The product data to add or update.</param>
        /// <returns>The index view if successful, otherwise the add/update view.</returns>
        [HttpPost]
        public async Task<IActionResult> Upsert(ProductUpsertViewModel productModel)
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
                        TempData["error"] = "Failed to add the image.";
                        return View(productModel);
                    }
                }

                TempData["success"] = $"Product {(productModel.Product.Id == Guid.Empty
                    ? "created" : "updated")} successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred. Please try again.";
                return View(productModel);
            }
        }

        #region API

        /// <summary>
        /// Checks if a product name is unique.
        /// </summary>
        /// <param name="product">The product DTO containing the name to check.</param>
        /// <returns>A JSON result indicating whether the name of the product is unique.</returns>
        [HttpGet]
        public async Task<IActionResult> IsProductNameUnique(ProductDto product)
        {
            if (product.Id != Guid.Empty)
            {
                var existingProduct = await _productGetterService.GetByIdAsync(product.Id);

                if (existingProduct is not null && existingProduct.Name == product.Name)
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

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A JSON result containing the list of products.</returns>
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
                    new { error = Constants.GENERIC_ERROR_MESSAGE });
            }
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>A JSON result indicating the success of the deletion operation.</returns>
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
                    throw new InvalidOperationException();
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
                    Message = "Failed to delete the product. Please try again later."
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Checks if a referenced entity has associations with products.
        /// </summary>
        /// <param name="type">The type of entity to check (e.g., category or manufacturer).</param>
        /// <param name="id">The ID of the entity to check.</param>
        /// <returns>A JSON result indicating whether the entity has associations with products.</returns>
        [HttpGet]
        public async Task<IActionResult> HasReference(string type, Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Id cannot be null."
                });
            }

            try
            {
                var products = type.ToLower() switch
                {
                    "category" => await _productGetterService.GetByCategoryAsync(id),
                    "manufacturer" => await _productGetterService.GetByManufacturerAsync(id),
                    _ => throw new ArgumentException("The referenced type is invalid.")
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
                    Message = Constants.GENERIC_ERROR_MESSAGE
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Removes references to an entity from products.
        /// </summary>
        /// <param name="type">The type of entity to remove references from (e.g., category or manufacturer).</param>
        /// <param name="id">The ID of the entity to remove references for.</param>
        /// <returns>A JSON result indicating the success of the operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveReference(string type, Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Id cannot be null."
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
                        throw new ArgumentException("The referenced type is invalid.");
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
                    Message = Constants.GENERIC_ERROR_MESSAGE
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Retrieves a sorted list of categories as select list items.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, 
        /// returning a sorted list of categories as select list items.</returns>
        private async Task<IEnumerable<SelectListItem>> GetCategoriesSelectList()
        {
            return _categorySorterService.Sort(await
                _categoryGetterService.GetAllAsync()).Select(t =>
                new SelectListItem() { Text = t.Name, Value = t.Id.ToString() });
        }

        /// <summary>
        /// Retrieves a sorted list of manufacturers as select list items.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, 
        /// returning a sorted list of manufacturers as select list items.</returns>
        private async Task<IEnumerable<SelectListItem>> GetManufacturersSelectList()
        {
            return _manufacturerSorterService.Sort(await
                _manufacturerGetterService.GetAllAsync()).Select(t =>
                new SelectListItem() { Text = t.Name, Value = t.Id.ToString() });
        }

        /// <summary>
        /// Retrieves select list items for price types.
        /// </summary>
        /// <returns>A list of select list items representing price types.</returns>
        private IEnumerable<SelectListItem> GetPriceTypesSelectList()
        {
            return Enum.GetNames(typeof(PriceType)).Select(
                t => new SelectListItem() { Text = t, Value = t });
        }

        #endregion
    }
}
