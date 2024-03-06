using Microsoft.AspNetCore.Mvc;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var product = await productRepository.GetAsync(id);
            return View(product);
        }
    }
}
