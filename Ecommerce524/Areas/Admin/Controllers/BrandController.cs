using Ecommerce524.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce524.Areas.Admin.Controllers
{
    
        [Area(SD.ADMIN_AREA)]
    public class  BrandController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public IActionResult Index(string? name, int page=1)
        {
            var brands = _context.Brands.AsNoTracking().AsQueryable(); 
            //filter
            if(name is not null)
            {
                brands = brands.Where(e => e.Name.Contains(name));
            }
            //pagination
            if (page < 1)
            {
                 page = 1;
            }

            int currentPage = page;
            double Pages = Math.Ceiling(brands.Count() / 5.0);

            brands = brands.Skip((page - 1) * 5).Take(5);


            return View(new BrandsVM
            {
                Brand = brands.AsEnumerable(),
                TotalPages = Pages,
                CurrentPage = currentPage,
            });
            
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var brand = _context.Brands.Find(1);
            if (brand is null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            _context.Brands.Update(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand is null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
