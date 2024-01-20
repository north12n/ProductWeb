using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductWeb.Models;

namespace ProductWeb.Date
{
    public class ProductContext : IdentityDbContext 
    {
        public ProductContext(DbContextOptions options ) : base( options ) 
        { 

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=TestProductWeb66; Trusted_Connection=True; TrustServerCertificate=True");
        }

        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
