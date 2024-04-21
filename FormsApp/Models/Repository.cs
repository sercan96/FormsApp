using FormsApp.Models.ContextClasses;
using FormsApp.Models.Entities;

namespace FormsApp.Models
{
    public class Repository
    {
        MyContext _db;

        public Repository(MyContext db)
        {
            _db = db;
        }

        public List<Category> Categories()
        {
            return _db.Categories.ToList();
        }

        public List<Product> Products()
        {
            return _db.Products.ToList();
        }
    }
}
