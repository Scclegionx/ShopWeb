using Microsoft.AspNetCore.Identity;
using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUser();
    }
}
