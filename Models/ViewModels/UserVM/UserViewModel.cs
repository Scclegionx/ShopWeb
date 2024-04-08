using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.UserVM
{
    public class UserViewModel
    {
        public List<User> Users { get; set; }
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ShipperRoleCheckBox { get; set; }
        public bool AdminRoleCheckBox { get; set; }
        [Required]
        public string Avatar { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
