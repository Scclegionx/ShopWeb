using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface IResponseRepository
    {
        Task<List<Response>> GetAllResponsesAsync();
        Task<Response> GetResponseByIdAsync(Guid id);
        Task<List<Response>> GetResponsesByUserIdAsync(Guid userId);
        Task<Response> AddResponseAsync(Response response);
        Task<Response> UpdateResponseAsync(Response response);
        Task<bool> DeleteResponseAsync(Guid id);
    }
}
