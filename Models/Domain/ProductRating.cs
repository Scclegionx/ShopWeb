namespace ShopWeb.Models.Domain
{
    public class ProductRating
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
    }
}
