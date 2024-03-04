namespace ShopWeb.Repositories
{
    public interface IImageRepository
    {
        Task<string> UploadAsync (IFormFile file);
    }
}
