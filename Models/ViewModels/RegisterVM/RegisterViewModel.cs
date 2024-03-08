using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.RegisterVM
{
    public class RegisterViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}
