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
            double TotalPages = Math.Ceiling(brands.Count() / 5.0);

            brands = brands.Skip((page - 1) * 5).Take(5);


            return View(new BrandsVM
            {
                Brand = brands.AsEnumerable(),
                TotalPages = TotalPages,
                CurrentPage = currentPage,
            });
            
        }
        [HttpGet] //to show the form
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost] //to handle the form submission
        //public IActionResult Create(Brand brand, IFormFile logo)
        //{
        //    _context.Brands.Add(brand);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}
        public IActionResult Create(Brand brand, IFormFile Logo)
        {
            if (Logo != null && Logo.Length > 0)
            {
                var newFileName = Guid.NewGuid().ToString() + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + Path.GetExtension(Logo.FileName);

                // var fileName = Path.GetFileName(Logo.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img/brand-logo", newFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Logo.CopyTo(stream);
                }

                brand.Logo = newFileName;
            }

            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet] //to show the form
        public IActionResult Edit(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand is null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost] //to handle the form submission
        public IActionResult Edit(Brand brand, IFormFile? Logo )
           
        {
            Brand? brandInDB = _context.Brands.AsNoTracking().FirstOrDefault(e=>e.Id == brand.Id);
            if (brandInDB is null)
            {
                return NotFound();
                
            }
            if (Logo is not null && Logo.Length > 0)
            {
                //crate new img
                var newFileName = Guid.NewGuid().ToString().Substring(0,7) + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + Path.GetExtension(Logo.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/brand-logo", newFileName); 


                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Logo.CopyTo(stream);
                }
                //delete old img
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/brand-logo", brandInDB.Logo); 
                
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
                //upddate to img in database
                brand.Logo = newFileName;

                
            }
            else
            {
                brand.Logo = brandInDB.Logo;
            }

                _context.Brands.Update(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet] //to show the form
        public IActionResult Delete(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null) {  return NotFound(); }

            var oldFilePath= Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img/brand-logo", brand.Logo);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                _context.SaveChanges();
            }

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            return RedirectToAction(nameof(Index));
        }
        //[HttpPost] //to handle the form submission
        //public IActionResult Delete(Brand brand)
        //{
        //    _context.Brands.Remove(brand);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
