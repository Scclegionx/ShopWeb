namespace ShopWeb.Models.Domain
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ICollection<CartItem> Items { get; set; }
    }
}
