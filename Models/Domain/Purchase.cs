namespace ShopWeb.Models.Domain
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string State { get; set; }
        public Guid? ShipperID { get; set; }
        public ICollection<PurchaseItem> Items { get; set; }
    }
}
