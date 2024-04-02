namespace ShopWeb.Models.ViewModels.LikeVM
{
    public class RemoveLikeRequest
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}
