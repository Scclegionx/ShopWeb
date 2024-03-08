namespace ShopWeb.Models.Domain
{
    public class ProductComment
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public DateTime TimeAdd { get; set; }
    }
}
