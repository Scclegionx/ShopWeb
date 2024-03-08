namespace ShopWeb.Models.ViewModels.LikeVM
{
    public class AddLikeRequest
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
    }
}
