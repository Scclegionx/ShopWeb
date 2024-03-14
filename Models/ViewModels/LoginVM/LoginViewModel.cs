using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.LoginVM
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống!")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
