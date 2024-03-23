using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.CartVM;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProductRepository productRepository;

        public CartController(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            this.userManager = userManager;
            this.productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get the current user's cart and map it to CartViewModel
            var userId = Guid.Parse(userManager.GetUserId(User)); // Implement this method to get the current user's ID
            var currentUserCart = await _cartRepository.GetCurrentUserCartAsync(userId);

            // Map the cart data to CartViewModel
            if (currentUserCart != null)
            {
                var CartIteminDomain = await _cartRepository.GetAllCartItems(currentUserCart.Id);
                var CartItemForView = new List<CartItemViewModel>();
                foreach (var item in CartIteminDomain)
                {
                    var productInCart = await productRepository.GetAsync(item.ProductId);
                    CartItemForView.Add(new CartItemViewModel
                    {
                        ProductName = productInCart.Name,
                        Price = productInCart.Price,
                        Quantity = item.Quantity
                    });
                }

                // Map the cart data to CartViewModel
                var cartViewModel = new CartViewModel
                {
                    Items = CartItemForView,
                    TotalPrice = currentUserCart.Items.Sum(item => item.Quantity * item.Product.Price)
                };

                return View(cartViewModel);
            }
            else
            {
                return View(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid productId, int quantity)
        {
            var userId = Guid.Parse(userManager.GetUserId(User)); // Implement this method to get the current user's ID
            var currentUserCart = await _cartRepository.GetCurrentUserCartAsync(userId);
            await _cartRepository.AddProductToCartAsync(currentUserCart, productId, quantity, userId);

            TempData["SuccessMessage"] = "Product successfully added to cart!";
            return RedirectToAction("Index");
        }
    }
}
