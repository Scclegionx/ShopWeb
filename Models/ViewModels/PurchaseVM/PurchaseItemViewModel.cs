using ShopWeb.Models.Domain;

namespace ShopWeb.Models.ViewModels.PurchaseVM
{
    public class PurchaseItemViewModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => Price * Quantity;
    }
}
