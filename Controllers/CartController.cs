using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.CartVM;
using ShopWeb.Models.ViewModels.PurchaseVM;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProductRepository productRepository;
        private readonly IVariantAttributesRepository variantAttributesRepository;
        private readonly IProductVariantRepository productVariantRepository;
        private readonly ICouponRepository couponRepository;
        private readonly IPurchaseRepository purchaseRepository;
        private readonly INotificationRepository notificationRepository;

        public CartController(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IProductRepository productRepository, IVariantAttributesRepository variantAttributesRepository,
            IProductVariantRepository productVariantRepository, ICouponRepository couponRepository, IPurchaseRepository purchaseRepository, INotificationRepository notificationRepository)
        {
            _cartRepository = cartRepository;
            this.userManager = userManager;
            this.productRepository = productRepository;
            this.variantAttributesRepository = variantAttributesRepository;
            this.productVariantRepository = productVariantRepository;
            this.couponRepository = couponRepository;
            this.purchaseRepository = purchaseRepository;
            this.notificationRepository = notificationRepository;
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

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(Guid productId, int quantity, Guid chosenVariant, string additionalParameter)
        {
            if (additionalParameter != null)
            {
                var totalPrice = 0;
                var product = await productRepository.GetAsync(productId);
                var listForView = new List<List<VariantAttribute>>();
                var mdls = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(chosenVariant);
                listForView.Add(mdls);
                var productVariant = await productVariantRepository.GetAsync(chosenVariant);
                if (productVariant != null)
                {
                    totalPrice = quantity * productVariant.Price;
                } else
                {
                    totalPrice = quantity * product.Price;
                }
                var model = new BuyOneViewModel
                {
                    Quantity = quantity,
                    Product = product,
                    TotalPrice = totalPrice,
                    Variants= listForView,
                    ProductVariant = productVariant
                };
                return View(model);
            }
            if (chosenVariant == Guid.Empty)
            {
                var userId = Guid.Parse(userManager.GetUserId(User)); // Implement this method to get the current user's ID
                var currentUserCart = await _cartRepository.GetCurrentUserCartAsync(userId);
                await _cartRepository.AddProductWithNoVariantToCartAsync(currentUserCart, productId, quantity, userId);

                TempData["SuccessMessage"] = "Product successfully added to cart!";
                return Json(new { success = true });
            } else
            {
                var userId = Guid.Parse(userManager.GetUserId(User)); // Implement this method to get the current user's ID
                var currentUserCart = await _cartRepository.GetCurrentUserCartAsync(userId);
                await _cartRepository.AddProductToCartAsync(currentUserCart, productId, quantity, chosenVariant, userId);

                TempData["SuccessMessage"] = "Product successfully added to cart!";
                return Json(new { success = true });
            }
        }
        [HttpPost("ConfirmOne")]
        public async Task<IActionResult> ConfirmOne(BuyOneViewModel buyOneViewModel, string paymentMethod)
        {
            var PM = "";
            if (paymentMethod == "CK")
            {
                PM = "Chuyển khoản";
            }
            else if (paymentMethod == "COD")
            {
                PM = "COD";
            }

            var purchase = new Purchase
            {
                UserId = Guid.Parse(userManager.GetUserId(User)),
                PurchaseDate = DateTime.Now,
                TotalPrice = buyOneViewModel.TotalPrice,
                PaymentMethod = PM,
                Address = buyOneViewModel.Address,
                Note = buyOneViewModel.Note,
                State = "None",
                ShipperID = null
            };


            await purchaseRepository.UpdatePurchaseCount(buyOneViewModel.ProductId, buyOneViewModel.Quantity);
            await productRepository.UpdateProductQuantity(buyOneViewModel.ProductId, buyOneViewModel.Quantity);



            await purchaseRepository.SavePurchaseAsync(purchase);

            var notification = new Notification
            {
                Title = "Đặt hàng thành công!",
                Message = "Đơn hàng của bạn đang được người gửi chuẩn bị.",
                CreatedAt = DateTime.Now,
                IsRead = false,
                UserId = Guid.Parse(userManager.GetUserId(User))
            };
            await notificationRepository.AddNotificationAsync(notification);

            // Redirect the user to the "Thank You" page
            return RedirectToAction("Index","Purchase");
        }

        [HttpPost("ApplyCouponOne")]
        public async Task<IActionResult> ApplyCouponOne(BuyOneViewModel model)
        {
            var discountAmount = await couponRepository.GetDiscountAmountByCodeAsync(model.CouponCode);
            var product = await productRepository.GetAsync(model.ProductId);
            var listForView = new List<List<VariantAttribute>>();
            var mdls = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(model.ProductVariantId);
            listForView.Add(mdls);
            var productVariant = await productVariantRepository.GetAsync(model.ProductVariantId);

            var mdl = new BuyOneViewModel
            {
                Quantity = model.Quantity,
                Product = product,
                TotalPrice = model.TotalPrice,
                Variants = listForView,
                ProductVariant = productVariant,
            };

            mdl.TotalPrice = mdl.TotalPrice - (mdl.TotalPrice * discountAmount);
            ModelState.Remove("TotalPrice");
            // Return the view with the updated model
            return View("AddToCart", mdl);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCartItem(Guid id)
        {
            await _cartRepository.DeleteCartItemAsync(id);
            return RedirectToAction("Index");
        }
        [HttpGet("GetCartItemCount")]
        public async Task<IActionResult> GetCartItemCount()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var userId = Guid.Parse(user.Id);
                var currentUserCart = await _cartRepository.GetCurrentUserCartAsync(userId);
                var cartNoti = await _cartRepository.GetItemCountInCart(currentUserCart.Id);

                return Json(new { itemCount = cartNoti });
            }

            return Json(new { itemCount = 0 });
        }
    }
}
