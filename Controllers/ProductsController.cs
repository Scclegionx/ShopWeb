using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IProductLikeRepository productLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IProductCommentRepository productCommentRepository;

        public ProductsController(IProductRepository productRepository, IProductLikeRepository productLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IProductCommentRepository productCommentRepository)
        {
            this.productRepository = productRepository;
            this.productLikeRepository = productLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.productCommentRepository = productCommentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var liked = false;
            var product = await productRepository.GetAsync(id);

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

                var productInDomain = await productCommentRepository.GetAllAsync(product.Id);

                var productCommentForView = new List<ProductCommentViewModel>();
                foreach (var productComment in productInDomain)
                {
                    productCommentForView.Add(new ProductCommentViewModel
                    {
                        Description = productComment.Description,
                        TimeAdd = productComment.TimeAdd,
                        Username = (await userManager.FindByIdAsync(productComment.UserId.ToString())).UserName
                    });
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
                    Comments = productCommentForView
                };
                ViewData["Liked"] = liked;
                return View(model);

            }
            ViewData["Liked"] = liked;
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
    }
}
