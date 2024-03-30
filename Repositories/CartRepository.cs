using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;

        public CartRepository(ShopWebDbContext shopWebDbContext)
        {
            this.shopWebDbContext = shopWebDbContext;
        }

        public async Task<Cart> GetCurrentUserCartAsync(Guid userId) 
        {
            return await shopWebDbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddProductToCartAsync(Cart cart, Guid productId, int quantity, Guid userId)
        {
            var product = await shopWebDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                // Handle invalid product ID
                return;
            }

            if (cart == null)
            {
                cart = new Cart { UserId = userId }; 
                shopWebDbContext.Carts.Add(cart);
            }

            cart.Items ??= new List<CartItem>();

            var existingItem = await shopWebDbContext.CartItems.FirstOrDefaultAsync(c => c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity });
            }

            await shopWebDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItems(Guid CartId)
        {
            return await shopWebDbContext.CartItems.Where(x => x.CartId == CartId).ToListAsync();
        }
    }
}
