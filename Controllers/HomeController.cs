using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models;
using ShopWeb.Models.ViewModels;
using ShopWeb.Repositories;
using System.Diagnostics;

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

        public async Task<IActionResult> Index()
        {
            var products = await productRepository.GetAllAsync();

            var cates = await cateRepository.GetAllAsync();

            var model = new HomeViewModel
            {
                Products = products,
                Categories = cates
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
    }
}
