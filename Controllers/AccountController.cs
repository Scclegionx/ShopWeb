﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.LoginVM;
using ShopWeb.Models.ViewModels.RegisterVM;
using ShopWeb.Models.ViewModels.UserVM;
using ShopWeb.Repositories;
using ShopWeb.Models.ViewModels.LikeVM;

namespace ShopWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IProductLikeRepository productLikeRepository;
        private readonly IProductRepository productRepository;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IProductLikeRepository productLikeRepository, IProductRepository productRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.productLikeRepository = productLikeRepository;
            this.productRepository = productRepository;
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
                // Check if the username already exists
                var existingUsername = await userManager.FindByNameAsync(registerViewModel.Username);
                if (existingUsername != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại!");
                    return View(registerViewModel);
                }

                // Check if the email already exists
                var existingEmail = await userManager.FindByEmailAsync(registerViewModel.Email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại!");
                    return View(registerViewModel);
                }

                // Check if the password meets the configured requirements
                var passwordValidator = new PasswordValidator<ApplicationUser>();
                var result = await passwordValidator.ValidateAsync(userManager, null, registerViewModel.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        switch (error.Code)
                        {
                            case "PasswordRequiresDigit":
                                ModelState.AddModelError("Password", "Mật khẩu phải chứa ít nhất 1 số!");
                                break;
                            case "PasswordRequiresLower":
                                ModelState.AddModelError("Password", "Mật khẩu phải chứa ít nhất 1 ký tự in thường!");
                                break;
                            case "PasswordRequiresUpper":
                                ModelState.AddModelError("Password", "Mật khẩu phải chứa ít nhất 1 ký tự in hoa!");
                                break;
                            case "PasswordRequiresNonAlphanumeric":
                                ModelState.AddModelError("Password", "Mật khẩu phải chứa ít nhất 1 ký tự đặc biệt!");
                                break;
                            case "PasswordRequiresUniqueChars":
                                ModelState.AddModelError("Password", "Password must contain at least one unique character.");
                                break;
                            case "PasswordTooShort":
                                ModelState.AddModelError("Password", "Độ dài tối thiểu của mật khẩu là 6 ký tự");
                                break;
                            default:
                                ModelState.AddModelError("Password", "Mật khẩu không hợp lệ!");
                                break;
                        }
                    }
                    return View(registerViewModel);
                }

                // Create new ApplicationUser
                var identityUser = new ApplicationUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email,
                    Avatar = "empty",
                    Address = "empty"
                };

                // Create the user
                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);
                if (identityResult.Succeeded)
                {
                    // Add user to role
                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
                    if (roleIdentityResult.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                }

                // If user creation or adding to role fails
                ModelState.AddModelError(string.Empty, "Failed to register user.");
            }

            // If ModelState is not valid, return to the view with errors
            return View(registerViewModel);
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
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

        [HttpGet]
        public async Task<IActionResult> LikedProducts()
        {
            // Get the currently logged-in user's ID
            Guid userId = Guid.Parse(userManager.GetUserId(User));

            // Fetch the liked products for the user
            var listOfId = await productLikeRepository.GetLikedProductIds(userId);

            var allLikedProducts = new List<Product>();

            foreach (var productId in listOfId)
            {
                var model = await productRepository.GetAsync(productId);
                if (model != null)
                {
                    allLikedProducts.Add(model);
                }
            }
            if (allLikedProducts != null)
            {
                var modelForView = new LikeProductsViewModel
                {
                    LikedProducts = allLikedProducts
                };
                return View(modelForView);
            } else
            {
                return View(null);
            }
        }

    }
}
