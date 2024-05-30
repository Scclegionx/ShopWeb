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
        private readonly IProductRatingRepository productRatingRepository;
        private readonly IProductCommentRepository productCommentRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICateRepository cateRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPurchaseRepository purchaseRepository,
            IProductRatingRepository productRatingRepository,
            IProductCommentRepository productCommentRepository)
        {
            _logger = logger;
            this.productRepository = productRepository;   
            this.cateRepository = cateRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.purchaseRepository = purchaseRepository;
            this.productRatingRepository = productRatingRepository;
            this.productCommentRepository = productCommentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string category, int page = 1)
        {
            

            if (signInManager.IsSignedIn(User) && User.IsInRole("Shipper"))
            {
                return RedirectToAction("Index", "Shipper");
            }
            var bestSellingProducts = await purchaseRepository.GetBestSellingProducts();

            const int pageSize = 10;

            // Get products based on category or all products if no category is provided
            IEnumerable<Product> products = string.IsNullOrEmpty(category)
                ? await productRepository.GetAllAsync(page, pageSize)
                : await productRepository.GetProductsByCategoryAsync(category);

            // Get total count for pagination
            var totalProductsCount = await productRepository.GetTotalProductsCount();

            // Calculate page count
            var pageCount = (int)Math.Ceiling((double)totalProductsCount / pageSize);


            products = await productRepository.GetAllAsync(page, pageSize);

            

            // Get categories for display
            var categories = await cateRepository.GetAllAsync(page, pageSize);

            foreach (var product in products) 
            {
                await productRepository.CheckProductAvailability(product.Id);
                
            }

            var productsByCategory = new Dictionary<string, List<Product>>();
            foreach (var categorys in categories)
            {
                var productsInCategory = await productRepository.GetProductsByCategoryForHomeAsync(categorys.Name);
                productsByCategory[categorys.Name] = productsInCategory;
            }

            var model = new HomeViewModel
            {
                Products = products,
                Categories = categories,
                PageNumber = page,
                PageCount = pageCount,
                bestSellingProducts = bestSellingProducts,
                ProductsByCategory = productsByCategory
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


            return PartialView("_ProductList", products);
        }

        [HttpGet]
        public async Task<IActionResult> SortByFeature(int page = 1, string sortBy = "Sales", bool sortDescending = false, string category = null)
        {
            const int pageSize = 9;
            // Get total count for pagination
            var totalProductsCount = await productRepository.GetTotalProductsCountAfterSort(category);

            // Calculate page count
            var pageCount = (int)Math.Ceiling((double)totalProductsCount / pageSize);
            ViewBag.page = page;
            ViewBag.pageCount = pageCount;
            var products = await productRepository.GetAllBySortAsync(page, pageSize, sortBy, sortDescending, category);

            return PartialView("_ProductList", products);
        }


        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Shop(string productName, string category, int page = 1)
        {
            const int pageSize = 9;

            // Get total count for pagination
            var totalProductsCount = await productRepository.GetTotalProductsCount();

            // Calculate page count
            var pageCount = (int)Math.Ceiling((double)totalProductsCount / pageSize);
            ViewBag.page = page;
            ViewBag.pageCount = pageCount;

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
            var categories = await cateRepository.GetAllAsync(page, 100);

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
            };
            return View(model);
        }

    }
}
