namespace ShopWeb.Models.Domain
{
    public class ProductLike
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
    }
}
