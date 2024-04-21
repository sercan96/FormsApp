namespace FormsApp.Models.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }

        // Relational Properties
        public virtual List<Product> Products { get; set; }

    }
}
