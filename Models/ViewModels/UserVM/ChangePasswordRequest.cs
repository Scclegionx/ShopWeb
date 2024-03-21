using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.UserVM
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Không được để trống!")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Không được để trống!")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Không được để trống!")]
        public string ConfirmPassWord { get; set; }
    }
}
