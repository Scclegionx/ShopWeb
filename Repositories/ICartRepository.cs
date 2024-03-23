using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCurrentUserCartAsync(Guid userId);
        Task AddProductToCartAsync(Cart cart, Guid productId, int quantity, Guid userId);
        Task< IEnumerable<CartItem> > GetAllCartItems(Guid CartId);
    }
}
