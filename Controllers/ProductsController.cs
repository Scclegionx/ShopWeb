using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.ProductVM;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IProductLikeRepository productLikeRepository;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProductCommentRepository productCommentRepository;
        private readonly IProductVariantRepository productVariantRepository;
        private readonly IVariantAttributesRepository variantAttributesRepository;
        private readonly IProductRatingRepository productRatingRepository;

        public ProductsController(IProductRepository productRepository, IProductLikeRepository productLikeRepository,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IProductCommentRepository productCommentRepository,
            IProductVariantRepository productVariantRepository,
            IVariantAttributesRepository variantAttributesRepository,
            IProductRatingRepository productRatingRepository)
        {
            this.productRepository = productRepository;
            this.productLikeRepository = productLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.productCommentRepository = productCommentRepository;
            this.productVariantRepository = productVariantRepository;
            this.variantAttributesRepository = variantAttributesRepository;
            this.productRatingRepository = productRatingRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var liked = false;
            var product = await productRepository.GetAsync(id);

            double averageRating = await productRatingRepository.GetAverageRating(id);

            if (product != null)
            {
                var totalLikes = await productLikeRepository.GetTotalLikes(product.Id);
                if (signInManager.IsSignedIn(User))
                {
                    var likesForProduct = await productLikeRepository.GetAllLikes(product.Id);
                    var userId = userManager.GetUserId(User);
                    if (userId != null)
                    {
                        var likeFromUser = likesForProduct.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }
                ViewData["TotalLikes"] = totalLikes;

                var productVariants = await productVariantRepository.GetVariantsByProductIdAsync(id);


                var listForView = new List<List<VariantAttribute>>();

                if (productVariants is not null)
                {
                    foreach (var productVariant in productVariants)
                    {
                        var mdls = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(productVariant.Id);
                        listForView.Add(mdls);
                    }
                }

                var productInDomain = await productCommentRepository.GetAllAsync(product.Id);

                var productCommentForView = new List<ProductCommentViewModel>();
                foreach (var productComment in productInDomain)
                {
                    var check = await userManager.FindByIdAsync(productComment.UserId.ToString());
                    if (check != null)
                    {
                        productCommentForView.Add(new ProductCommentViewModel
                        {
                            Description = productComment.Description,
                            TimeAdd = productComment.TimeAdd,
                            Username = check.UserName
                        });
                    } else
                    {
                        productCommentForView.Add(new ProductCommentViewModel
                        {
                            Description = productComment.Description,
                            TimeAdd = productComment.TimeAdd,
                            Username = "User Deleted"
                        });
                    }
                }

                var model = new ProductDetailViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    FeaturedImageUrl = product.FeaturedImageUrl,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Categories = product.Categories,
                    ProductLike = product.ProductLike,
                    Comments = productCommentForView,
                    Variants = listForView
                };
                ViewData["Liked"] = liked;
                ViewData["AverageRating"] = averageRating;
                return View(model);

            }
            ViewData["Liked"] = liked;
            ViewData["AverageRating"] = averageRating;
            return View();

            
            
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductDetailViewModel productDetailViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var model = new ProductComment
                {
                    ProductId = productDetailViewModel.Id,
                    Description = productDetailViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    TimeAdd = DateTime.Now,
                };
                await productCommentRepository.AddAsync(model);
                return RedirectToAction("Index", "Products");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RateProduct(ProductDetailViewModel model)
        {
            // Get the current user's ID
            var userId = Guid.Parse(userManager.GetUserId(User));
            var productId = model.Id;

            // Check if the user has already rated the product
            var existingRating = await productRatingRepository.GetRatingByUserAndProduct(userId, productId);
            if (existingRating != null)
            {
                // If the user has already rated the product, update their rating
                existingRating.Rating = model.Rating;
                await productRatingRepository.UpdateAsync(existingRating);
            }
            else
            {
                // If the user hasn't rated the product yet, add a new rating
                var productRating = new ProductRating
                {
                    UserId = userId,
                    ProductId = productId,
                    Rating = model.Rating
                };
                await productRatingRepository.AddAsync(productRating);
            }

            // Redirect back to the product details page
            return RedirectToAction("Index", "Home");
        }
    }
}
