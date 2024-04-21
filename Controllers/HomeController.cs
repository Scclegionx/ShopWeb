using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels;
using ShopWeb.Repositories;
using System.Diagnostics;
using PagedList;
using Microsoft.AspNetCore.Identity;

namespace ShopWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository productRepository;
        private readonly ICateRepository cateRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IPurchaseRepository purchaseRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICateRepository cateRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPurchaseRepository purchaseRepository)
        {
            _logger = logger;
            this.productRepository = productRepository;   
            this.cateRepository = cateRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.purchaseRepository = purchaseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string productName, string category, int page = 1)
        {
            if (signInManager.IsSignedIn(User) && User.IsInRole("Shipper"))
            {
                return RedirectToAction("Index", "Shipper");
            }
            var bestSellingProducts = await purchaseRepository.GetBestSellingProducts();

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

            foreach (var product in products) 
            {
                await productRepository.CheckProductAvailability(product.Id);
            }

            var model = new HomeViewModel
            {
                Products = products,
                Categories = categories,
                PageNumber = page,
                PageCount = pageCount,
                bestSellingProducts = bestSellingProducts,
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
