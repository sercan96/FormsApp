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

        public void Save()
        {
            _db.SaveChanges();
        }

        public List<Category> Categories()
        {
            return _db.Categories.ToList();
        }

        public List<Product> Products()
        {
            return _db.Products.ToList();
        }

        public void EditProduct(Product updateProduct)
        {
            var entity = _db.Products.FirstOrDefault(p=> p.ProductId == updateProduct.ProductId);
            if (entity != null)
            {
                entity.Name = updateProduct.Name;
                entity.Price = updateProduct.Price;
                entity.Image = updateProduct.Image;
                entity.CategoryId = updateProduct.CategoryId;
                entity.IsActive = updateProduct.IsActive;
                _db.Update(entity);
                Save();
            }
        }

        public void Add(Product model)
        {
            _db.Products.Add(model);
        }

        public void Delete(Product model)
        {
            _db.Remove(model);
        }
    }
}
