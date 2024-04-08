using ShopWeb.Models.Domain;

namespace ShopWeb.Models.ViewModels.PurchaseVM
{
    public class AcceptPurchaseRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string PaymentMethod { get; set; }
        public string State { get; set; }
        public Guid? ShipperID { get; set; }
        public List<Purchase> Purchases { get; set; }
    }
}
