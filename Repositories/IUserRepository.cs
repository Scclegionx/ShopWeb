using Microsoft.AspNetCore.Identity;

namespace ShopWeb.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllUser();
    }
}
