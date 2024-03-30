namespace ShopWeb.Models.Domain
{
    public class Coupon
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
