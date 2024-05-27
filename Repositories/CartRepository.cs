using Microsoft.Ajax.Utilities;
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

        public async Task AddProductWithNoVariantToCartAsync(Cart cart, Guid productId, int quantity, Guid userId)
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

            var existingItem = await shopWebDbContext.CartItems.Where(x => x.CartId == cart.Id).FirstOrDefaultAsync(c => c.ProductId == productId);

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

        public async Task AddProductToCartAsync(Cart cart, Guid productId, int quantity, Guid productVariantId, Guid userId)
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

            var existingItem = await shopWebDbContext.CartItems
                .Where(x => x.CartId == cart.Id && x.ProductId == productId && x.ProductVariantId == productVariantId)
                .FirstOrDefaultAsync();

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity, ProductVariantId = productVariantId });
            }

            await shopWebDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItems(Guid CartId)
        {
            return await shopWebDbContext.CartItems.Where(x => x.CartId == CartId).ToListAsync();
        }

        public async Task<CartItem> ClearCartItemsAsync(Guid cartId)
        {
            var cartItems = await shopWebDbContext.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();
            shopWebDbContext.CartItems.RemoveRange(cartItems);
            await shopWebDbContext.SaveChangesAsync();
            return null;
        }

        public async Task<Cart> ClearCartAsync(Cart cart)
        {
            shopWebDbContext.Carts.Remove(cart);
            await shopWebDbContext.SaveChangesAsync();
            return null;
        }

        public async Task<Cart?> DeleteCartItemAsync(Guid cartItemId)
        {
            var cartItem = await shopWebDbContext.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                shopWebDbContext.CartItems.Remove(cartItem);
                await shopWebDbContext.SaveChangesAsync();
            }
            return null;
        }

        public async Task<int> GetItemCountInCart(Guid cartId)
        {
            return await shopWebDbContext.CartItems
            .Where(item => item.CartId == cartId)
            .CountAsync();
        }
    }
}
