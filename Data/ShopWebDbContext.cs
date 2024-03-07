using Microsoft.EntityFrameworkCore;
using ShopWeb.Models.Domain;
namespace ShopWeb.Data
{
    public class ShopWebDbContext : DbContext
    {
        public ShopWebDbContext(DbContextOptions<ShopWebDbContext> options) : base(options) 
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductLike> ProductLike { get; set; }
    }
}
