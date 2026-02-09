using Ecommerce524.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce524.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } 
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; } 
        public DbSet<ProductColor> ProductColors { get; set; } 
        public DbSet<ProductSubImg> ProductSubImgs { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Database=Ecommerce524;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //        "Server=.;Database=Ecommerce524;Integrated Security=True;TrustServerCertificate=True"
        //    );
        //}

    }
}
