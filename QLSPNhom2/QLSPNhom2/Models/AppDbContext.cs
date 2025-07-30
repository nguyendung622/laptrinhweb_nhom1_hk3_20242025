using Microsoft.EntityFrameworkCore;

namespace QLSPNhom2.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            //Add-Migration Name
            //Remove-Migration
            //Update-Database
        }
    }
}
