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
        private readonly IVariantAttributesRepository variantAttributesRepository;
        private readonly IProductVariantRepository productVariantRepository;
        private readonly IImageRepository imageRepository;

        public AdminProductsController(ICateRepository cateRepository, IProductRepository productRepository, 
            IVariantAttributesRepository variantAttributesRepository, IProductVariantRepository productVariantRepository,
            IImageRepository imageRepository) 
        {
            this.cateRepository = cateRepository;
            this.productRepository = productRepository;
            this.variantAttributesRepository = variantAttributesRepository;
            this.productVariantRepository = productVariantRepository;
            this.imageRepository = imageRepository;
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
                State = "Available",
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

            // Handle additional image URLs
            if (addProductRequest.AdditionalImageUrls != null && addProductRequest.AdditionalImageUrls.Any())
            {
                foreach (var imageUrl in addProductRequest.AdditionalImageUrls)
                {
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        var productImage = new ProductImage
                        {
                            ImageUrl = imageUrl,
                            ProductId = model.Id
                        };
                        await productRepository.AddImageAsync(productImage);
                    }
                }
            }

            // Add variant attributes to the database
            if (addProductRequest.VariantAttributes != null && addProductRequest.VariantAttributes.Any())
            {
                int numAttributes = addProductRequest.VariantAttributes[0].Name.Count();
                foreach (var variantAttributeRequest in addProductRequest.VariantAttributes)
                {
                    var productVariantId = Guid.NewGuid();
                    var productVariant = new ProductVariant
                    {
                        Id = productVariantId,
                        ProductId = model.Id,
                        Price = variantAttributeRequest.Price,
                        Quantity = variantAttributeRequest.Quantity,
                    };

                    // Add product variant to the database
                    await productVariantRepository.AddAsync(productVariant);

                    // Add variant attributes for the product variant
                    for (int i = 0; i < numAttributes; i++)
                    {
                        var variantAttribute = new VariantAttribute
                        {
                            Key = addProductRequest.VariantAttributes[0].Name[i],
                            Value = variantAttributeRequest.Value[i],
                            ProductVariantId = productVariant.Id,
                        };
                        await variantAttributesRepository.AddAsync(variantAttribute);
                    }
                }
            }

            return RedirectToAction("List", "AdminProducts");
        }
        [HttpGet]
        public async Task<IActionResult> List(int page = 1)
        {
            const int pageSize = 10;
            var totalProductsCount = await productRepository.GetTotalProductsCount();
            var pageCount = (int)Math.Ceiling((double)totalProductsCount / pageSize);
            var products = await productRepository.GetAllAsync(page,pageSize);

            ViewBag.page = page;
            ViewBag.pageCount = pageCount;

            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await productRepository.GetAsync(id);
            var categoriesDomainModel = await cateRepository.GetAllAsync();
            var allVariantProducts = await productVariantRepository.GetVariantsByProductIdAsync(id);
            var listVAForView = new List<VariantAttributeRequest>();
            foreach (var variant in allVariantProducts)
            {
                var listName = new List<string>();
                var listValue = new List<string>();
                var allVA = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(variant.Id);
                foreach (var variantAttribute in allVA)
                {
                    listName.Add(variantAttribute.Key);
                    listValue.Add(variantAttribute.Value);
                }
                var mdl = new VariantAttributeRequest
                {
                    Name = listName,
                    Value = listValue,
                    Price = variant.Price,
                    Quantity = variant.Quantity,
                };
                listVAForView.Add(mdl);
            }
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
                    VariantAttributes = listVAForView,
                    AdditionalImageUrls = product.ProductImages.Select(pi => pi.ImageUrl).ToList()
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
                Quantity = editProductRequest.Quantity
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

            // Handle additional image URLs
            if (editProductRequest.AdditionalImageUrls != null && editProductRequest.AdditionalImageUrls.Any())
            {
                // Get existing additional images for the product
                var existingAdditionalImages = await productRepository.GetAdditionalImagesByProductIdAsync(editProductRequest.Id);

                // Update existing additional images with the new list
                foreach (var existingImage in existingAdditionalImages)
                {
                    // Remove existing image from the database
                    await productRepository.DeleteImageAsync(existingImage);
                }
                foreach (var imageUrl in editProductRequest.AdditionalImageUrls)
                {
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        var productImage = new ProductImage
                        {
                            ImageUrl = imageUrl,
                            ProductId = model.Id
                        };
                        await productRepository.AddImageAsync(productImage);
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
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var product = await productRepository.GetAsync(id);
            var categoriesDomainModel = await cateRepository.GetAllAsync();
            var allVariantProducts = await productVariantRepository.GetVariantsByProductIdAsync(id);
            var listVAForView = new List<VariantAttributeRequest>();
            foreach (var variant in allVariantProducts)
            {
                var listName = new List<string>();
                var listValue = new List<string>();
                var allVA = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(variant.Id);
                foreach (var variantAttribute in allVA)
                {
                    listName.Add(variantAttribute.Key);
                    listValue.Add(variantAttribute.Value);
                }
                var mdl = new VariantAttributeRequest
                {
                    Name = listName,
                    Value = listValue,
                    Price = variant.Price,
                    Quantity = variant.Quantity,
                    ProductVariantId = variant.Id,
                };
                listVAForView.Add(mdl);
            }
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
                    VariantAttributes = listVAForView,
                    AdditionalImageUrls = product.ProductImages.Select(pi => pi.ImageUrl).ToList()
                };
                return View(model);
            }
            return View(null);
        }
        [HttpGet]
        public async Task<IActionResult> EditVariant(Guid id)
        {
            var variant = await productVariantRepository.GetAsync(id);
            if (variant == null)
            {
                return NotFound();
            }

            var variantAttributes = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(id);
            var model = new EditVariantRequest
            {
                VariantId = variant.Id,
                Price = variant.Price,
                Quantity = variant.Quantity,
                ProductId = variant.ProductId,
                VariantAttributes = variantAttributes.Select(va => new VariantAttributeRequestForEdit
                {
                    Key = va.Key,
                    Value = va.Value
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditVariant(EditVariantRequest editVariantRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(editVariantRequest);
            }

            var variant = await productVariantRepository.GetAsync(editVariantRequest.VariantId);
            if (variant == null)
            {
                return NotFound();
            }

            variant.Price = editVariantRequest.Price;
            variant.Quantity = editVariantRequest.Quantity;

            await productVariantRepository.UpdateAsync(variant);

            // Update variant attributes
            foreach (var variantAttributeRequest in editVariantRequest.VariantAttributes)
            {
                // Get the existing variant attribute by ProductVariantId and Key
                var existingVariantAttributes = await variantAttributesRepository.GetByProductVariantIdAsync(editVariantRequest.VariantId);

                // Find the matching variant attribute by Key
                var existingVariantAttribute = existingVariantAttributes.FirstOrDefault(va => va.Key == variantAttributeRequest.Key);

                // If the existing variant attribute is found, update its value
                if (existingVariantAttribute != null)
                {
                    existingVariantAttribute.Value = variantAttributeRequest.Value;
                    await variantAttributesRepository.UpdateAsync(existingVariantAttribute);
                }
            }


            return RedirectToAction("Detail", "AdminProducts", new { id = variant.ProductId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteVariant(Guid id)
        {
            var variant = await productVariantRepository.GetAsync(id);
            if (variant == null)
            {
                return NotFound();
            }

            await productVariantRepository.DeleteAsync(id);

            // Also delete variant attributes associated with this variant
            var variantAttributes = await variantAttributesRepository.GetAllVariantsAttributeByVariantAsync(id);
            foreach (var variantAttribute in variantAttributes)
            {
                await variantAttributesRepository.DeleteAsync(variantAttribute.Id);
            }

            return RedirectToAction("Detail", "AdminProducts", new { id = variant.ProductId });
        }


    }
}
