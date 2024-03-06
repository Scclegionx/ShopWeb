using ShopWeb.Models.Domain;

namespace ShopWeb.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set;}
    }
}
