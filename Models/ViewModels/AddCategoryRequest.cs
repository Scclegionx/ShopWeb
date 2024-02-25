using ShopWeb.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels
{
    public class AddCategoryRequest
    {
        [Display(Name = "Tên Danh Mục")]
        public string Name { get; set; }
        [Display(Name = "Mô Tả")]
        public string Description { get; set; }
    }
}
