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
        public async Task<IActionResult> Index(string productName, int page = 1)
        {

            const int pageSize = 3; // Number of products per page


            // Total number of products (for pagination)
            var totalProductsCount = await productRepository.GetTotalProductsCount();
            var pageCount = (int)Math.Ceiling((double)totalProductsCount / pageSize);

            if (productName != null)
            {
                var productSearch = await productRepository.FindByNameAsync(productName);
                var cates = await cateRepository.GetAllAsync();
                var model = new HomeViewModel
                {
                    Products = productSearch,
                    Categories = cates,
                    PageNumber = page,
                    PageCount = pageCount
                };
                return View(model);

            } else
            {
                var products = await productRepository.GetAllAsync(page, pageSize);

                var cates = await cateRepository.GetAllAsync();

                var model = new HomeViewModel
                {
                    Products = products,
                    Categories = cates,
                    PageNumber = page,
                    PageCount = pageCount
                };

                return View(model);
            }
            
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
