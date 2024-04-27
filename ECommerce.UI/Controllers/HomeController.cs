using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerce.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductGetterService _productGetterService;

        public HomeController(IProductGetterService productGetterService)
        {
            _productGetterService = productGetterService;
        }

        public async Task<IActionResult> Index(int page = 1, ProductPageModel? productPage = null)
        {
            var products = await _productGetterService
                .GetBySearchQueryAsync(productPage?.SearchQuery);
            var productsPerPage = 10;
            int totalPages;

            if (products.Count != 0 && products.Count % productsPerPage == 0)
            {
                totalPages = products.Count / productsPerPage;
            }
            else
            {
                totalPages = products.Count / productsPerPage + 1;
            }

            var currentPage = (page < 1 || page > totalPages) ? 1 : page;
            var adjacentPagesCount = 2;
            var paginationStart = Math.Max(currentPage - adjacentPagesCount, 1);
            var paginationEnd = Math.Min(currentPage + adjacentPagesCount, totalPages);

            var pageStartIndex = (currentPage - 1) * productsPerPage;
            var productsOnCurrentPage = Math.Min(products.Count - pageStartIndex, 10);
            var productsForDisplay = products.GetRange(pageStartIndex, productsOnCurrentPage);

            var productPageModel = new ProductPageModel()
            {
                Products = productsForDisplay,
                CurrentPage = currentPage,
                PaginationStart = paginationStart,
                PaginationEnd = paginationEnd,
                TotalPages = totalPages
            };

            return View(productPageModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
