using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels;
using ShopWeb.Repositories;
using System.Diagnostics;
using PagedList;

namespace ShopWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository productRepository;
        private readonly ICateRepository cateRepository;
        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICateRepository cateRepository)
        {
            _logger = logger;
            this.productRepository = productRepository;   
            this.cateRepository = cateRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string productName, string category, int page = 1)
        {
            const int pageSize = 10;

            // Get total count for pagination
            var totalProductsCount = await productRepository.GetTotalProductsCount();

            // Calculate page count
            var pageCount = (int)Math.Ceiling((double)totalProductsCount / pageSize);

            IEnumerable<Product> products;

            // Check if both productName and category are provided
            if (!string.IsNullOrEmpty(productName) && !string.IsNullOrEmpty(category))
            {
                // Find products matching both name and category
                products = await productRepository.FindByNameAndCategoryAsync(productName, category);
            }
            // If only productName is provided
            else if (!string.IsNullOrEmpty(productName))
            {
                // Find products by name
                products = await productRepository.FindByNameAsync(productName);
            }
            // If only category is provided
            else if (!string.IsNullOrEmpty(category))
            {
                // Find products by category
                products = await productRepository.GetProductsByCategoryAsync(category);
            }
            // If neither productName nor category is provided, fetch all products
            else
            {
                products = await productRepository.GetAllAsync(page, pageSize);
            }

            // Get categories for display
            var categories = await cateRepository.GetAllAsync();

            var model = new HomeViewModel
            {
                Products = products,
                Categories = categories,
                PageNumber = page,
                PageCount = pageCount
            };

            return View(model);
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

        [HttpGet]
        public async Task<IActionResult> SortByCategory(string category)
        {
            var products = await productRepository.GetProductsByCategoryAsync(category);

            var model = new HomeViewModel
            {
                Products = products,
                Categories = await cateRepository.GetAllAsync()
            };

            return PartialView("_ProductList", model);
        }

    }
}
