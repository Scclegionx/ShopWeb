using ShopWeb.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels.CategoryVM
{
    public class AddCategoryRequest
    {
        [Required]
        [Display(Name = "Tên Danh Mục")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Mô Tả")]
        public string Description { get; set; }
    }
}
