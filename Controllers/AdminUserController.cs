using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.ViewModels.UserVM;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUserController(IUserRepository userRepository,
            UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }

        [HttpGet]
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
        [HttpPost]
        public async Task<IActionResult> List(UserViewModel userViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = userViewModel.Username,
                Email = userViewModel.Email,
            };

            var identityResult = await userManager.CreateAsync(identityUser, userViewModel.Password);
            
            if (identityResult is not null)
            {
                if (identityResult.Succeeded)
                {
                    var roles = new List<string> { "User" };

                    if (userViewModel.AdminRoleCheckBox)
                    {
                        roles.Add("Admin");
                    }

                    await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUser");
                    }
                }
            }
            //Show error
            return RedirectToAction("List", "AdminUser");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is not null)
            {
                var identityResult = await userManager.DeleteAsync(user);
                if (identityResult is not null)
                {
                    return RedirectToAction("List", "AdminUser");
                }
            }
            return View();
        }
    }
}
