using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.ResponseVM
{
    public class ResponseViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "User Id")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "The heading field is required.")]
        public string Heading { get; set; }

        [Required(ErrorMessage = "The content field is required.")]
        public string Content { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        public string? UserName { get; set; }
    }
}
