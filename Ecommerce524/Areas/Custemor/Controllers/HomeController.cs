using System.Diagnostics;
using Ecommerce524.Models;

using Ecommerce524.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce524.Areas.Custemor.Controllers
{
    [Area(SD.CUSTEMOR_AREA)]
    public class HomeController : Controller
    {
        
        private ApplicationDbContext _context = new ApplicationDbContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int? categoryId)
        {
           // const double discount = 50; 

            var  products = _context.Products
                .Include(e => e.Category)
                .Where(e => e.Discount > 50 );

          
            if (categoryId is not null)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            products = products
                .Skip(0)
                .Take(16);

            var  categories = _context.Categories.Include(e=>e.Products)
                .AsQueryable();

            return View(model: new ProductsWithCategoriesVM
            {
                Products = products.ToList(),
                Categories = categories.ToList()
            });
        }

        public IActionResult Details(int id)
        {
           var product =  _context.Products.SingleOrDefault(e => e.Id == id);

           if(product is null)
            {
                return RedirectToAction(nameof(NotFoundPage));
            }

            var sameCategories = _context.Products
                 .Where(e => e.CategoryId == product.CategoryId && e.Id != product.Id)
                 .Skip(0)
                 .Take(4);
            var minPrice = product.price - product.price * (10m / 100m);
            var maxPrice = product.price + product.price * (10m / 100m);

            var samePrices = _context.Products
                .Where(e=>e.price >= minPrice && e.price <= maxPrice && e.Id != product.Id)
                 .Skip(0)
                 .Take(4);
            // return View(product);
            var relatedProducts = _context.Products
                 .Where(e => e.Name.Contains(product.Name) && e.Id != product.Id)
                 .Skip(0)
                 .Take(4);
            return View(new ProductWithRelatedVM
            {
                sameCategories = sameCategories.ToList(),
                Product = product, 
                RelatedProducts = relatedProducts.ToList(),
                samePrices = samePrices.ToList()
            });

        }


        public IActionResult NotFoundPage()
        {
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
