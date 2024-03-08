using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.ViewModels.UserVM;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly IUserRepository userRepository;

        public AdminUserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<IActionResult> List()
        {
            var users = await userRepository.GetAllUser();
            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();
            foreach (var user in users)
            {
                usersViewModel.Users.Add(new User
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    Username = user.UserName
                });
            }
            return View(usersViewModel);
        }
    }
}
