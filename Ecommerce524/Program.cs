using Ecommerce524.DataAccess;
using Ecommerce524.Repositories;
using Ecommerce524.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ecommerce524
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // ? 1. Register DbContext FIRST (?????)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ? 2. Register Repository (???? ??? Service ??????)
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
          
            // ? 3. Register Services (???? ??? Controller ???????)
            builder.Services.AddScoped<IBrandService, BrandService>();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<CategoryService>();

            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();  // ? ??? ??? ????? ??? ???? ?????? ????
             
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Custemor}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}