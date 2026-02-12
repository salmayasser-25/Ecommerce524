using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce524.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public IActionResult Index(string? name, int page = 1)
        {
            var category = _context.Categories.AsQueryable();  
            //add new filter
            if (name is not null)
            {
                category = category.Where(e => e.Name.Contains(name));
            }
            //pagination
            if (page < 1)
            {
                page = 1;
            }
            int currentPage = page;
            double Pages = Math.Ceiling(category.Count() / 5.0);

            category = category.Skip((page - 1) * 5).Take(5);
            return View(new CategoriesVM
            {
                Category = category.AsEnumerable(),
                
                TotalPages = Pages,
                CurrentPage = currentPage,
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Create(string name, string? description, bool status)
        //{

        //    return View();

        //    //}

        [HttpPost] 
        public IActionResult Create(Categories category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Categories category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
