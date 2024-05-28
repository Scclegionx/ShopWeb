using ShopWeb.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.CartVM
{
    public class BuyOneViewModel
    {
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public decimal TotalPrice { get; set; }
        [Display(Name = "Mã giảm giá")]
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<List<VariantAttribute>> Variants { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductVariantId { get; set; }
    }
}
