using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCurrentUserCartAsync(Guid userId);
        Task AddProductToCartAsync(Cart cart, Guid productId, int quantity, Guid productVariantId, Guid userId);
        Task AddProductWithNoVariantToCartAsync(Cart cart, Guid productId, int quantity, Guid userId);
        Task< IEnumerable<CartItem> > GetAllCartItems(Guid CartId);
        Task<CartItem> ClearCartItemsAsync(Guid cartId);
        Task<Cart> ClearCartAsync(Cart cart);
        Task<Cart?> DeleteCartItemAsync(Guid cartItemId);
        Task<int> GetItemCountInCart(Guid cartId);
    }
}
