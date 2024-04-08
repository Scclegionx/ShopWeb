using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.CartVM;
using ShopWeb.Models.ViewModels.PurchaseVM;
using ShopWeb.Models.ViewModels.UserVM;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository purchaseRepository;
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICouponRepository couponRepository;

        public PurchaseController(IPurchaseRepository purchaseRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            UserManager<ApplicationUser> userManager, ICouponRepository couponRepository
            )
        {
            this.purchaseRepository = purchaseRepository;
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.userManager = userManager;
            this.couponRepository = couponRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(userManager.GetUserId(User));
            var purchases = await purchaseRepository.GetOwnPurchases(userId);
            return View(purchases);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid Id)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                var currentUserPurchase = await purchaseRepository.GetPurchaseById(Id);

                if (currentUserPurchase != null)
                {
                    var PurchaseIteminDomain = await purchaseRepository.GetAllPurchaseItems(currentUserPurchase.Id);
                    var PurchaseItemForView = new List<PurchaseItemViewModel>();
                    foreach (var item in PurchaseIteminDomain)
                    {
                        var productInCart = await productRepository.GetAsync(item.ProductId);
                        PurchaseItemForView.Add(new PurchaseItemViewModel
                        {
                            ProductName = productInCart.Name,
                            Price = productInCart.Price,
                            Quantity = item.Quantity
                        });
                    }

                    // Map the cart data to PurchaseViewModel
                    var purchaseViewModel = new PurchaseViewModel
                    {
                        PurchaseItems = PurchaseItemForView,
                        TotalPrice = currentUserPurchase.TotalPrice,
                    };

                    return View(purchaseViewModel);
                }
                else
                {
                    return View(null);
                }
            } else
            {
                var currentUserPurchase = await purchaseRepository.GetPurchaseById(Id);

                if (currentUserPurchase != null)
                {
                    var PurchaseIteminDomain = await purchaseRepository.GetAllPurchaseItems(currentUserPurchase.Id);
                    var PurchaseItemForView = new List<PurchaseItemViewModel>();
                    foreach (var item in PurchaseIteminDomain)
                    {
                        var productInCart = await productRepository.GetAsync(item.ProductId);
                        PurchaseItemForView.Add(new PurchaseItemViewModel
                        {
                            ProductName = productInCart.Name,
                            Price = productInCart.Price,
                            Quantity = item.Quantity
                        });
                    }

                    // Map the cart data to PurchaseViewModel
                    var purchaseViewModel = new PurchaseViewModel
                    {
                        PurchaseItems = PurchaseItemForView,
                        TotalPrice = currentUserPurchase.TotalPrice,
                    };

                    return View(purchaseViewModel);
                }
                else
                {
                    return View(null);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await purchaseRepository.DeletePurchaseAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> BuyNow()
        {
            var userId = Guid.Parse(userManager.GetUserId(User)); 
            var currentUserCart = await cartRepository.GetCurrentUserCartAsync(userId);

            if (currentUserCart != null)
            {
                var CartIteminDomain = await cartRepository.GetAllCartItems(currentUserCart.Id);
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

                var cartViewModel = new PurchaseViewModel
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
        public async Task<IActionResult> Confirm(PurchaseViewModel purchaseViewModel, string paymentMethod)
        {
            var PM = "";
            if (paymentMethod == "CK")
            {
                PM = "Chuyển khoản";
            } else if (paymentMethod == "COD")
            {
                PM = "COD";
            }
            var userId = Guid.Parse(userManager.GetUserId(User));
            var currentUserCart = await cartRepository.GetCurrentUserCartAsync(userId);

            var purchase = new Purchase
            {
                UserId = Guid.Parse(userManager.GetUserId(User)),
                PurchaseDate = DateTime.Now,
                TotalPrice = purchaseViewModel.TotalPrice,
                PaymentMethod = PM,
                Address = purchaseViewModel.Address,
                Note = purchaseViewModel.Note,
                State = "None",
                ShipperID = null
            };

            await purchaseRepository.SavePurchaseAsync(purchase);

            if (currentUserCart != null)
            {
                var CartItems = await cartRepository.GetAllCartItems(currentUserCart.Id);
                foreach (var item in CartItems)
                {
                    var purchaseItem = new PurchaseItem
                    {
                        PurchaseId = purchase.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                        // Add any other relevant data from the cart item to the purchase item
                    };
                    await purchaseRepository.AddPurchaseItem(purchaseItem);
                }
            }

            // Clear the cart and cart items associated with the current user
            await cartRepository.ClearCartItemsAsync(currentUserCart.Id);
            await cartRepository.ClearCartAsync(currentUserCart);

            // Redirect the user to the "Thank You" page
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(PurchaseViewModel model)
        {
            var discountAmount = await couponRepository.GetDiscountAmountByCodeAsync(model.CouponCode);

            
            if (model.Items == null)
            {
                var userId = Guid.Parse(userManager.GetUserId(User));
                var currentUserCart = await cartRepository.GetCurrentUserCartAsync(userId);

                if (currentUserCart != null)
                {
                    var CartIteminDomain = await cartRepository.GetAllCartItems(currentUserCart.Id);
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
                    model.Items = CartItemForView;
                    // Recalculate the total price by subtracting the discount amount
                    model.TotalPrice = model.TotalPrice - (model.TotalPrice * discountAmount);
                }
            }
            ModelState.Remove("TotalPrice");
            ModelState.AddModelError("TotalPrice", model.TotalPrice.ToString());

            // Return the view with the updated model
            return View("BuyNow", model);
        }
        [HttpGet]
        public async Task<IActionResult> PurchaseManage()
        {
            var purchases = await purchaseRepository.GetAllPurchases();
            return View(purchases);
        }
    }
}
