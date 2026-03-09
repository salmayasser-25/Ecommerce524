using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ecommerce524.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce524.ViewModel;

namespace Ecommerce524.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSubImg> ProductSubImgs { get; set; }
        public DbSet<Ecommerce524.ViewModel.RegisterVM> RegisterVM { get; set; } = default!;
    }
}
