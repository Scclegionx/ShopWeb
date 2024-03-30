using ShopWeb.Models.ViewModels.CartVM;

namespace ShopWeb.Models.ViewModels.PurchaseVM
{
    public class PurchaseViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItemViewModel> Items { get; set; }
        public List<PurchaseItemViewModel> PurchaseItems { get; set; }
        public string PaymentMethod { get; set; }
        public string CouponCode { get; set; } 
        public decimal DiscountAmount { get; set; }
    }
}
