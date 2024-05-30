using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;
        private readonly IProductRatingRepository productRatingRepository;

        public ProductRepository(ShopWebDbContext shopWebDbContext, IProductRatingRepository productRatingRepository)
        {
            this.shopWebDbContext = shopWebDbContext;
            this.productRatingRepository = productRatingRepository;
        }
        public async Task<Product> AddAsync(Product product)
        {
            await shopWebDbContext.Products.AddAsync(product);
            await shopWebDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(Guid id)
        {
            var existingProd = await shopWebDbContext.Products.FindAsync(id);
            if (existingProd != null)
            {
                shopWebDbContext.Products.Remove(existingProd);
                await shopWebDbContext.SaveChangesAsync();
                return existingProd;
            }
            return null;
        }

        public async Task<IEnumerable<Product?>> FindByNameAsync(string productName)
        {
            var existProducts = await shopWebDbContext.Products.Where(x => x.Name.Contains(productName)).ToListAsync();
            return existProducts;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int? page, int? pageSize)
        {
            // Calculate the number of items to skip
            if (page.HasValue && pageSize.HasValue)
            {
                int skip = (page.Value - 1) * pageSize.Value;
                return await shopWebDbContext.Products.Include(x => x.Categories).Skip(skip).Take(pageSize.Value).ToListAsync();
            }
            return await shopWebDbContext.Products.Include(x => x.Categories).ToListAsync();

        }

        public async Task<Product?> GetAsync(Guid id)
        {
            return await shopWebDbContext.Products.Include(x => x.ProductImages).Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);    
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existingProd = await shopWebDbContext.Products.Include(x => x.Categories).Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == product.Id);
            if (existingProd != null)
            {
                existingProd.Id = product.Id;
                existingProd.Name = product.Name;
                existingProd.Description = product.Description;
                existingProd.FeaturedImageUrl = product.FeaturedImageUrl;
                existingProd.Price = product.Price;
                existingProd.Quantity = product.Quantity;
                existingProd.Categories = product.Categories;

                await shopWebDbContext.SaveChangesAsync();
                return existingProd;
            }
            return null;
        }

        public async Task<Product?> UpdateRatingAsync (Guid id)
        {
            double? averageR = await productRatingRepository.GetAverageRating(id);
            if (averageR == null) 
            {
                averageR = 0;
            }
            var roundedRating = Math.Round((decimal)averageR * 2) / 2;

            var existingProd = await shopWebDbContext.Products.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
            if (existingProd != null)
            {
                existingProd.Rating = (int)roundedRating;

                await shopWebDbContext.SaveChangesAsync();
                return existingProd;
            }
            return null;

        }
        public async Task<Product?> UpdateCommentCountAsync (Guid id, int totalComments)
        {
            var existingProd = await shopWebDbContext.Products.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
            if (existingProd != null)
            {
                existingProd.CommentsCount = totalComments;

                await shopWebDbContext.SaveChangesAsync();
                return existingProd;
            }
            return null;
        }

        public async Task<int> GetTotalProductsCount()
        {
            return await shopWebDbContext.Products.CountAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            var products = await shopWebDbContext.Products
                .Where(p => p.Categories.Any(c => c.Name == category))
                .ToListAsync();

            return products;
        }
        public async Task<List<Product>> GetProductsByCategoryForHomeAsync(string category)
        {
            var products = await shopWebDbContext.Products
                .Where(p => p.Categories.Any(c => c.Name == category))
                .OrderByDescending(p => p.PurchaseCount)
                .Take(3)
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<Product>> FindByNameAndCategoryAsync(string productName, string category)
        {
            return await shopWebDbContext.Products
                .Include(p => p.Categories)
                .Where(p => p.Name.Contains(productName) && p.Categories.Any(c => c.Name == category))
                .ToListAsync();
        }

        public async Task UpdateProductQuantity(Guid productId, int quantity)
        {
            var product = await shopWebDbContext.Products.FindAsync(productId);
            if (product != null)
            {
                product.Quantity -= quantity;
                await shopWebDbContext.SaveChangesAsync();
            }
        }
        public async Task CheckProductAvailability(Guid productId)
        {
            var product = await shopWebDbContext.Products.FindAsync(productId);
            if (product != null)
            {
                if (product.Quantity <= 0)
                {
                    product.State = "Out of Order";
                }
                else
                {
                    product.State = "Available";
                }
                await shopWebDbContext.SaveChangesAsync();
            }
        }

        public async Task AddImageAsync(ProductImage productImage)
        {
            await shopWebDbContext.ProductImages.AddAsync(productImage);
            await shopWebDbContext.SaveChangesAsync();
        }

        public async Task<List<ProductImage>> GetAdditionalImagesAsync(Guid productId)
        {
            return await shopWebDbContext.ProductImages.Where(pi => pi.ProductId == productId).ToListAsync();
        }

        public async Task<List<ProductImage>> GetAdditionalImagesByProductIdAsync(Guid productId)
        {
            return await shopWebDbContext.ProductImages.Where(pi => pi.ProductId == productId).ToListAsync();
        }

        public async Task DeleteImageAsync(ProductImage productImage)
        {
            shopWebDbContext.ProductImages.Remove(productImage);
            await shopWebDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetSaleProductsAsync()
        {
            return await shopWebDbContext.Products.Where(p => p.IsSale && p.SaleEndDate > DateTime.Now).ToListAsync();
        }

        public async Task SetSaleAsync(Guid productId, decimal salePrice, DateTime saleEndDate)
        {
            var product = await shopWebDbContext.Products.FindAsync(productId);
            if (product != null)
            {
                product.IsSale = true;
                product.SalePrice = salePrice;
                product.SaleEndDate = saleEndDate;
                await shopWebDbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveSaleAsync(Guid productId)
        {
            var product = await shopWebDbContext.Products.FindAsync(productId);
            if (product != null)
            {
                product.IsSale = false;
                product.SalePrice = null;
                product.SaleEndDate = null;
                await shopWebDbContext.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<Product>> GetAllBySortAsync(int page, int pageSize, string sortBy, bool sortDescending, string category = null)
        {
            var query = shopWebDbContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Categories.Any(c => c.Name == category));
            }

            switch (sortBy)
            {
                case "Sales":
                    query = query.Where(p => p.IsSale);
                    break;
                case "AtoZ":
                    query = sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;
                case "Price":
                    query = sortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                    break;
            }

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> GetTotalProductsCountAfterSort(string category = null)
        {
            var query = shopWebDbContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Categories.Any(c => c.Name == category));
            }

            return await query.CountAsync();
        }
    }
}
