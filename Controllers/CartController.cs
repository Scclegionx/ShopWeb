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
        private readonly IVariantAttributesRepository variantAttributesRepository;
        private readonly IProductVariantRepository productVariantRepository;

        public CartController(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IProductRepository productRepository, IVariantAttributesRepository variantAttributesRepository,
            IProductVariantRepository productVariantRepository)
        {
            _cartRepository = cartRepository;
            this.userManager = userManager;
            this.productRepository = productRepository;
            this.variantAttributesRepository = variantAttributesRepository;
            this.productVariantRepository = productVariantRepository;
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
                if (CartIteminDomain.Count() == 0)
                {
                    return View(null);
                }
                var CartItemForView = new List<CartItemViewModel>();
                foreach (var item in CartIteminDomain)
                {
                    var listForView = new List<List<VariantAttribute>>();
                    var productInCart = await productRepository.GetAsync(item.ProductId);
                    var mdls = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(item.ProductVariantId);
                    listForView.Add(mdls);


                    var productVariantPrice = await productVariantRepository.GetAsync(item.ProductVariantId);
                    if (productVariantPrice is null)
                    {
                        CartItemForView.Add(new CartItemViewModel
                        {
                            Id = item.Id,
                            ProductName = productInCart.Name,
                            Price = productInCart.Price,
                            Quantity = item.Quantity,
                            Variants = listForView
                        });
                    } else
                    {
                        CartItemForView.Add(new CartItemViewModel
                        {
                            Id = item.Id,
                            ProductName = productInCart.Name,
                            Price = productVariantPrice.Price,
                            Quantity = item.Quantity,
                            Variants = listForView
                        });
                    }

                }
                decimal totalPrice = 0;
                foreach (var cartItem in CartItemForView)
                {
                    totalPrice += cartItem.Price * cartItem.Quantity;
                }

                // Map the cart data to CartViewModel

                var cartViewModel = new CartViewModel
                {
                    Items = CartItemForView,
                    TotalPrice = totalPrice
                };

                return View(cartViewModel);
            }
            else
            {
                return View(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid productId, int quantity, Guid chosenVariant)
        {
            if (chosenVariant == Guid.Empty)
            {
                var userId = Guid.Parse(userManager.GetUserId(User)); // Implement this method to get the current user's ID
                var currentUserCart = await _cartRepository.GetCurrentUserCartAsync(userId);
                await _cartRepository.AddProductWithNoVariantToCartAsync(currentUserCart, productId, quantity, userId);

                TempData["SuccessMessage"] = "Product successfully added to cart!";
                return RedirectToAction("Index");
            } else
            {
                var userId = Guid.Parse(userManager.GetUserId(User)); // Implement this method to get the current user's ID
                var currentUserCart = await _cartRepository.GetCurrentUserCartAsync(userId);
                await _cartRepository.AddProductToCartAsync(currentUserCart, productId, quantity, chosenVariant, userId);

                TempData["SuccessMessage"] = "Product successfully added to cart!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCartItem(Guid id)
        {
            await _cartRepository.DeleteCartItemAsync(id);
            return RedirectToAction("Index");
        }
    }
}
