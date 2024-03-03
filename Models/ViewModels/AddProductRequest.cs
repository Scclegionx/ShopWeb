using Microsoft.AspNetCore.Mvc.Rendering;
using ShopWeb.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models.ViewModels
{
    public class AddProductRequest
    {
        [Display(Name = "Tên Mặt Hàng")]
        public string Name { get; set; }
        [Display(Name = "Mô Tả")]
        public string? Description { get; set; }
        [Display(Name = "Đường Dẫn Ảnh")]
        public string FeaturedImageUrl { get; set; }
        [Display(Name = "Giá")]
        public int Price { get; set; }
        [Display(Name = "Số Lượng")]
        public int Quantity { get; set; }
        [Display(Name = "Danh Mục Mặt Hàng")]

        //Display Cates
        public IEnumerable<SelectListItem> Categories { get; set; }
        //Collect Cates
        public string[] SelectedCategory {  get; set; } = Array.Empty<string>();
    }
}
