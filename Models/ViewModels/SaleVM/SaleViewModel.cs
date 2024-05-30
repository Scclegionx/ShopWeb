using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.SaleVM
{
    public class SaleViewModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal OriginalPrice { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid sale price")]
        public decimal SalePrice { get; set; }

        [Required]
        public DateTime SaleEndDate { get; set; }
    }
}
