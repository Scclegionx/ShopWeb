namespace ShopWeb.Models.Domain
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid CartId { get; set; }
        public Product Product { get; set; }
    }
}
