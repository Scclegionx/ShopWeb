using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.LoginVM
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
