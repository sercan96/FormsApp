using FormsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormsApp.Models.ContextClasses
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
