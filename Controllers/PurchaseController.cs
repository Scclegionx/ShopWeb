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
        private readonly IVariantAttributesRepository variantAttributesRepository;
        private readonly IProductVariantRepository productVariantRepository;

        public PurchaseController(IPurchaseRepository purchaseRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            UserManager<ApplicationUser> userManager, ICouponRepository couponRepository,
            IVariantAttributesRepository variantAttributesRepository,
            IProductVariantRepository productVariantRepository
            )
        {
            this.purchaseRepository = purchaseRepository;
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.userManager = userManager;
            this.couponRepository = couponRepository;
            this.variantAttributesRepository = variantAttributesRepository;
            this.productVariantRepository = productVariantRepository;
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
                        var listForView = new List<List<VariantAttribute>>();
                        var mdls = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(item.ProductVariantId);
                        listForView.Add(mdls);
                        var productInCart = await productRepository.GetAsync(item.ProductId);
                        var productVariantPrice = await productVariantRepository.GetAsync(item.ProductVariantId);
                        if (productVariantPrice is null)
                        {
                            PurchaseItemForView.Add(new PurchaseItemViewModel
                            {
                                ProductName = productInCart.Name,
                                Price = productInCart.Price,
                                Quantity = item.Quantity,
                                Variants = listForView
                            });
                        }
                        else
                        {
                            PurchaseItemForView.Add(new PurchaseItemViewModel
                            {
                                ProductName = productInCart.Name,
                                Price = productVariantPrice.Price,
                                Quantity = item.Quantity,
                                Variants = listForView
                            });
                        }
                    }
                    decimal totalPrice = 0;
                    foreach (var cartItem in PurchaseItemForView)
                    {
                        totalPrice += cartItem.Price * cartItem.Quantity;
                    }

                    // Map the cart data to PurchaseViewModel
                    var purchaseViewModel = new PurchaseViewModel
                    {
                        PurchaseItems = PurchaseItemForView,
                        TotalPrice = totalPrice,
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
                        var listForView = new List<List<VariantAttribute>>();
                        var mdls = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(item.ProductVariantId);
                        listForView.Add(mdls);
                        var productInCart = await productRepository.GetAsync(item.ProductId);
                        var productVariantPrice = await productVariantRepository.GetAsync(item.ProductVariantId);
                        if (productVariantPrice is null)
                        {
                            PurchaseItemForView.Add(new PurchaseItemViewModel
                            {
                                ProductName = productInCart.Name,
                                Price = productInCart.Price,
                                Quantity = item.Quantity,
                                Variants = listForView
                            });
                        } else
                        {
                            PurchaseItemForView.Add(new PurchaseItemViewModel
                            {
                                ProductName = productInCart.Name,
                                Price = productVariantPrice.Price,
                                Quantity = item.Quantity,
                                Variants = listForView
                            });
                        }
                    }

                    decimal totalPrice = 0;
                    foreach (var cartItem in PurchaseItemForView)
                    {
                        totalPrice += cartItem.Price * cartItem.Quantity;
                    }

                    // Map the cart data to PurchaseViewModel
                    var purchaseViewModel = new PurchaseViewModel
                    {
                        PurchaseItems = PurchaseItemForView,
                        TotalPrice = totalPrice,
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
                    var listForView = new List<List<VariantAttribute>>();
                    var mdls = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(item.ProductVariantId);
                    listForView.Add(mdls);
                    var productInCart = await productRepository.GetAsync(item.ProductId);
                    var productVariantPrice = await productVariantRepository.GetAsync(item.ProductVariantId);
                    if (productVariantPrice is null)
                    {
                        CartItemForView.Add(new CartItemViewModel
                        {
                            ProductName = productInCart.Name,
                            Price = productInCart.Price,
                            Quantity = item.Quantity,
                            Variants = listForView,
                            ProductId = productInCart.Id,
                            ProductVariantId = item.ProductVariantId,
                        });
                    } else
                    {
                        CartItemForView.Add(new CartItemViewModel
                        {
                            ProductName = productInCart.Name,
                            Price = productVariantPrice.Price,
                            Quantity = item.Quantity,
                            Variants = listForView,
                            ProductId = productInCart.Id,
                            ProductVariantId = item.ProductVariantId,
                        });
                    }
                }

                decimal totalPrice = 0;
                foreach (var cartItem in CartItemForView)
                {
                    totalPrice += cartItem.Price * cartItem.Quantity;
                }

                var cartViewModel = new PurchaseViewModel
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

            var listProductId = new List<Guid>();
            var listQuantity = new List<int>();
            var listProductVariantId = new List<Guid>();

            foreach (var id in purchaseViewModel.ProductId)
            {
                listProductId.Add(id);
            }

            foreach (var id in purchaseViewModel.ProductVariantId)
            {
                listProductVariantId.Add(id);
            }

            foreach (var quan in purchaseViewModel.Quantity) { listQuantity.Add(quan); }

            for (var i = 0; i < listProductId.Count; i++)
            {
                await purchaseRepository.UpdatePurchaseCount(listProductId[i], listQuantity[i]);
                await productRepository.UpdateProductQuantity(listProductId[i], listQuantity[i]);
                await variantAttributesRepository.UpdateProductVariantQuantity(listProductVariantId[i], listQuantity[i]);
            }


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
                        Quantity = item.Quantity,
                        ProductVariantId = item.ProductVariantId,
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
                        var listForView = new List<List<VariantAttribute>>();
                        var mdls = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(item.ProductVariantId);
                        listForView.Add(mdls);
                        var productVariantPrice = await productVariantRepository.GetAsync(item.ProductVariantId);
                        var productInCart = await productRepository.GetAsync(item.ProductId);
                        if (productVariantPrice != null)
                        {
                            CartItemForView.Add(new CartItemViewModel
                            {
                                ProductName = productInCart.Name,
                                Price = productVariantPrice.Price,
                                Quantity = item.Quantity,
                                Variants = listForView
                            });
                        } else
                        {
                            CartItemForView.Add(new CartItemViewModel
                            {
                                ProductName = productInCart.Name,
                                Price = productInCart.Price,
                                Quantity = item.Quantity,
                                Variants = listForView
                            });
                        }
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
            var donePurchases = purchases.Where(p => p.State == "Done").ToList();
            var otherPurchases = purchases.Where(p => p.State != "Done").ToList();

            ViewData["donePurchases"] = donePurchases;
            ViewData["otherPurchases"] = otherPurchases;


            return View(purchases);
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var userId = Guid.Parse(userManager.GetUserId(User));
            var models = await purchaseRepository.GetOwnPurchaseForHistory(userId);
            return View(models);
        }
        [HttpGet]
        public async Task<IActionResult> Tracking()
        {
            var userId = Guid.Parse(userManager.GetUserId(User));
            var models = await purchaseRepository.GetOwnPurchaseForTracking(userId);
            return View(models);
        }
    }
}
