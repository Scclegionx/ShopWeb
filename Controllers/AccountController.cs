using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.LoginVM;
using ShopWeb.Models.ViewModels.RegisterVM;
using ShopWeb.Models.ViewModels.UserVM;

namespace ShopWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new ApplicationUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email,
                };
                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

                if (identityResult.Succeeded) 
                {
                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

                    if (roleIdentityResult.Succeeded)
                    {
                        return RedirectToAction("Login");
                        //them thong bao
                    }
                }
            }


            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = ReturnUrl };

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

                if (signInResult.Succeeded && signInResult != null)
                {
                    if(!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UserDetail()
        {
            var user = await userManager.GetUserAsync(User);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditAccountRequest editAccountRequest)
        {
            if (!ModelState.IsValid)
            {
                // If model state is not valid, return to the edit view with validation errors
                return View(editAccountRequest);
            }

            // Get the current user
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                // If user not found, return Unauthorized or handle as needed
                return Unauthorized();
            }

            // Update user properties
            currentUser.Avatar = editAccountRequest.Avatar;
            currentUser.Email = editAccountRequest.Email;
            currentUser.Address = editAccountRequest.Address;

            // Update user in the database
            var result = await userManager.UpdateAsync(currentUser);
            if (!result.Succeeded)
            {
                // If update failed, add errors to model state and return to the edit view
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(editAccountRequest);
            }

            // If update successful, redirect to a success page or another action
            return RedirectToAction("UserDetail", "Account");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassWord(ChangePasswordRequest changePasswordRequest)
        {
            
            if (changePasswordRequest.NewPassword != changePasswordRequest.ConfirmPassWord)
            {
                // If the passwords don't match, add a model error and return to the view
                ModelState.AddModelError("ConfirmPassword", "The new password and confirm password do not match.");
                return View(changePasswordRequest); // Assuming the view is bound to ChangePasswordRequest
            }
            if (!ModelState.IsValid)
            {
                // If model state is not valid, return bad request
                return View(changePasswordRequest);
            }

            // Get the current user
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                // If user not found, return unauthorized
                return Unauthorized();
            }
            var signInResult = await signInManager.PasswordSignInAsync(currentUser.UserName, changePasswordRequest.OldPassword, isPersistent: false, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                // If the current password is incorrect, add a model error and return to the view
                ModelState.AddModelError("OldPassword", "The current password is incorrect.");
                return View(changePasswordRequest); // Assuming the view is bound to ChangePasswordRequest
            }

            // Verify the current password
            var result = await userManager.ChangePasswordAsync(currentUser, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
            if (!result.Succeeded)
            {
                // If password change failed, add errors to model state and return bad request
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }

            // If password change successful, sign out the user and redirect to a success page or login page
            await signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "Password successfully changed.";

            return RedirectToAction("Login", "Account");
        }
    }
}
