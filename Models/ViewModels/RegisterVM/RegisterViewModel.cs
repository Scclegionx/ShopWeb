using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.RegisterVM
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Mật khẩu")]
        [MinLength(6, ErrorMessage ="Mật khẩu phải có độ dài tối thiểu 6 ký tự!")]
        public string Password { get; set; }
    }
}
