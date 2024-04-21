namespace ShopWeb.Models.ViewModels.ProductVM
{
    public class VariantAttributeRequestForEdit
    {
        public Guid Id { get; set; } // This is the ID of the variant attribute in the database
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
