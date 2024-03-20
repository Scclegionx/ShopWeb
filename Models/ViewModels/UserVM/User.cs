using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.UserVM
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        [Required] 
        public string Avatar { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
