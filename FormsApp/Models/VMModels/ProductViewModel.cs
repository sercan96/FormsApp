using FormsApp.Models.Entities;

namespace FormsApp.Models.VMModels
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; } = null;
        public List<Category> Categories { get; set; } = null;
        public string? SelectedCategory { get; set; }
    }
}
