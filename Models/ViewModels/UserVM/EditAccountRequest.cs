using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.UserVM
{
    public class EditAccountRequest
    {

        [Required(ErrorMessage = "Không được để trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Avatar { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Address { get; set; }
    }
}
