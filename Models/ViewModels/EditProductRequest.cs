using Microsoft.AspNetCore.Mvc.Rendering;
using ShopWeb.Models.Domain;

namespace ShopWeb.Models.ViewModels
{
    public class EditProductRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FeaturedImageUrl { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string[] SelectedCategory { get; set; } = Array.Empty<string>();
    }
}
