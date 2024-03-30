using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.CouponVM
{
    public class CreateCouponViewModel
    {
        [Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Discount amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be greater than or equal to 0.")]
        public decimal DiscountAmount { get; set; }

        [Required(ErrorMessage = "Expiration date is required.")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
    }
}
