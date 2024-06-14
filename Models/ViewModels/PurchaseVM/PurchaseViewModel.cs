using ShopWeb.Models.ViewModels.CartVM;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Mã giảm giá")]
        public string CouponCode { get; set; } 
        public decimal DiscountAmount { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        public List<Guid> ProductId { get; set; }
        public List<int> Quantity { get; set; }
        public List<Guid> ProductVariantId { get; set; }
    }
}
