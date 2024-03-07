namespace ShopWeb.Models.ViewModels
{
    public class AddLikeRequest
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
    }
}
