using Microsoft.AspNetCore.Mvc;
using ShopWeb.Repositories;
using ShopWeb.Models.ViewModels.SaleVM;
namespace ShopWeb.Controllers
{
    public class AdminSaleController : Controller
    {
        private readonly IProductRepository productRepository;

        public AdminSaleController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ManageSales()
        {
            var products = await productRepository.GetAllAsync(1, 10);
            var saleProducts = products.Where(p => p.IsSale).Select(p => new SaleViewModel
            {
                ProductId = p.Id,
                ProductName = p.Name,
                OriginalPrice = p.Price,
                SalePrice = p.SalePrice ?? p.Price,
                SaleEndDate = p.SaleEndDate ?? DateTime.Now
            }).ToList();

            return View(saleProducts);
        }

        [HttpGet]
        public async Task<IActionResult> SetSale()
        {
            var products = await productRepository.GetAllAsync(1, 10);
            ViewBag.Products = products.Select(p => new { p.Id, p.Name }).ToList();

            return View(new SaleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SetSale(SaleViewModel model)
        {
            await productRepository.SetSaleAsync(model.ProductId, model.SalePrice, model.SaleEndDate);
            return RedirectToAction("ManageSales");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSale(Guid productId)
        {
            await productRepository.RemoveSaleAsync(productId);
            return RedirectToAction("ManageSales");
        }
    }
}
