using Microsoft.AspNetCore.Mvc;
using ShopWeb.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.CategoryVM;
using ShopWeb.Models.ViewModels.ProductVM;

namespace ShopWeb.Controllers
{
    public class AdminProductsController : Controller
    {
        private readonly ICateRepository cateRepository;
        private readonly IProductRepository productRepository;
        public AdminProductsController(ICateRepository cateRepository, IProductRepository productRepository) 
        {
            this.cateRepository = cateRepository;
            this.productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var cates = await cateRepository.GetAllAsync();

            var model = new AddProductRequest
            {
                Categories = cates.Select(c => new SelectListItem { Text = c.Description , Value = c.Id.ToString() } ),
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddProductRequest addProductRequest)
        {
            var model = new Product
            {
                Name = addProductRequest.Name,
                Description = addProductRequest.Description,
                FeaturedImageUrl = addProductRequest.FeaturedImageUrl,
                Price = addProductRequest.Price,
                Quantity = addProductRequest.Quantity,
            };
            var selectedCates = new List<Category>();
            foreach (var selectedCateId in addProductRequest.SelectedCategory)
            {
                var selectedCateIdAsGuid = Guid.Parse(selectedCateId);
                var existingCate = await cateRepository.GetAsync(selectedCateIdAsGuid);
                if (existingCate != null)
                {
                    selectedCates.Add(existingCate);
                }
            }
            model.Categories = selectedCates;
            await productRepository.AddAsync(model);
            return RedirectToAction("List", "AdminProducts");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var products = await productRepository.GetAllAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await productRepository.GetAsync(id);
            var categoriesDomainModel = await cateRepository.GetAllAsync();
            if (product != null)
            {
                var model = new EditProductRequest
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    FeaturedImageUrl = product.FeaturedImageUrl,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Categories = categoriesDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
                    SelectedCategory = product.Categories.Select(x => x.Id.ToString()).ToArray(), 
                };
                return View(model);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditProductRequest editProductRequest)
        {
            var model = new Product
            {
                Id = editProductRequest.Id,
                Name = editProductRequest.Name,
                Description = editProductRequest.Description,
                FeaturedImageUrl = editProductRequest.FeaturedImageUrl,
                Price = editProductRequest.Price,
                Quantity = editProductRequest.Quantity,
            };
            var selectedCates = new List<Category>();
            foreach(var selectedCate in editProductRequest.SelectedCategory)
            {
                if (Guid.TryParse(selectedCate, out var cate))
                {
                    var foundCate = await cateRepository.GetAsync(cate);
                    if (foundCate != null)
                    {
                        selectedCates.Add(foundCate);
                    }
                }
            }
            model.Categories = selectedCates;
            var updated = await productRepository.UpdateAsync(model);
            if (updated != null)
            {
                return RedirectToAction("List", "AdminProducts");
            }
            return RedirectToAction("List", "AdminProducts");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await productRepository.GetAsync(id);
            var categoriesDomainModel = await cateRepository.GetAllAsync();
            if (product != null)
            {
                var model = new EditProductRequest
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    FeaturedImageUrl = product.FeaturedImageUrl,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Categories = categoriesDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
                    SelectedCategory = product.Categories.Select(x => x.Id.ToString()).ToArray(),
                };
                return View(model);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditCategoryRequest editCategoryRequest)
        {
            var deletedProd = await productRepository.DeleteAsync(editCategoryRequest.Id);
            if (deletedProd != null)
            {
                return RedirectToAction("List", "AdminProducts");
            } else
            {
                return View(null);
            }
        }
    }
}
