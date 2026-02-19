using Ecommerce524.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce524.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class ProductController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public IActionResult Index(ProductFilterVM productFilterVM, int page = 1)
        {
            var products = _context.Products.AsQueryable();

            // Include relationships
            products = products.Include(e => e.Category).Include(e => e.Brand);

            // Get categories and brands for filter dropdowns
            var categories = _context.Categories.AsNoTracking().AsQueryable();
            var brands = _context.Brands.AsNoTracking().AsQueryable();

            // Create a new instance for filter values to return to view
            ProductFilterVM productFileResponse = new ProductFilterVM();

            //filter with name 
            if (productFilterVM.Name is not null)
            {
                products = products.Where(e => e.Name.Contains(productFilterVM.Name));
                productFileResponse.Name = productFilterVM.Name;
            }


            //filter min max price
            decimal? nullableMinPrice = productFilterVM.MinPrice;
            if (nullableMinPrice.HasValue && nullableMinPrice.Value > 0)
            {
                products = products.Where(e => e.price >= nullableMinPrice.Value);
                productFileResponse.MinPrice = (long?)nullableMinPrice.Value;
            }
            decimal? nullableMaxPrice = productFilterVM.MaxPrice;
            if (nullableMaxPrice.HasValue && nullableMaxPrice.Value > 0)
            {
                products = products.Where(e => e.price <= nullableMaxPrice.Value);
                productFileResponse.MaxPrice = (long?)nullableMaxPrice.Value;
            }
            // Apply filters based on incoming productFilterVM
            if (productFilterVM.CategoryId is not null)
            {
                products = products.Where(e => e.CategoryId == productFilterVM.CategoryId);
                productFileResponse.CategoryId = productFilterVM.CategoryId;
            }

            if (productFilterVM.BrandId is not null)
            {
                products = products.Where(e => e.BrandId == productFilterVM.BrandId);
                productFileResponse.BrandId = productFilterVM.BrandId;
            }
            if (productFilterVM.LessQuantity)
            {
                products = products.Where(e => e.Quantity < 50);
                productFileResponse.LessQuantity = productFilterVM.LessQuantity;
            }

            // Pagination
            if (page < 1)
            {
                page = 1;
            }
            int currentPage = page;
            double Pages = Math.Ceiling(products.Count() / 5.0);

            products = products.Skip((page - 1) * 5).Take(5);

            // Create the view model
            var productsVM = new ProductsVM
            {
                Products = products.AsEnumerable(),
                Categories = categories.AsEnumerable(),
                Brands = brands.AsEnumerable(),
                CurrentPage = currentPage,
                TotalPages = Pages,
                productFilterVM = productFileResponse // Use productFileVM that contains the applied filters
            };

            return View(productsVM);
        }
        //العك من اول هنا 
        [HttpGet]
        public IActionResult Create()
        {

            var categories = _context.Categories.ToList();
            var brands = _context.Brands.ToList();

            ViewBag.Categories = categories;
            ViewBag.Brands = brands;

            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product, IFormFile MainImg, List<IFormFile>? SubImgs)
        {
            if (MainImg != null && MainImg.Length > 0)
            {
                var newFileName = Guid.NewGuid().ToString()
                    + DateTime.UtcNow.ToString("yyyyMMddHHmmss")
                    + Path.GetExtension(MainImg.FileName);

                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/img/Product",
                    newFileName
                );

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    MainImg.CopyTo(stream);
                }

                product.MainImg = newFileName;
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            if (SubImgs != null)
            {
                foreach (var file in SubImgs)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString()
                            + Path.GetExtension(file.FileName);

                        var path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot/img/Product/Sub",
                            fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        var subImg = new ProductSubImg
                        {
                            SubImg = fileName,
                            ProductId = product.Id
                        };

                        _context.ProductSubImgs.Add(subImg);
                    }
                }

                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

       
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products
                       .Include(p => p.SubImages) 
                       .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();

            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product, IFormFile? MainImg, List<IFormFile>? SubImgs)
        {
            var productInDB = _context.Products
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == product.Id);

            if (productInDB == null)
            {
                return NotFound();
            }


            if (MainImg != null && MainImg.Length > 0)
            {

                if (!string.IsNullOrEmpty(productInDB.MainImg))
                {
                    var oldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/img/Product",
                        productInDB.MainImg
                    );

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }


                var newFileName = Guid.NewGuid().ToString()
                                  + DateTime.UtcNow.ToString("yyyyMMddHHmmss")
                                  + Path.GetExtension(MainImg.FileName);

                var newPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/img/Product",
                    newFileName
                );

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    MainImg.CopyTo(stream);
                }

                product.MainImg = newFileName;
            }
            else
            {

                product.MainImg = productInDB.MainImg;
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            if (SubImgs != null && SubImgs.Count > 0)

            {
                foreach (var file in SubImgs)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString()
                            + Path.GetExtension(file.FileName);

                        var path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot/img/Product/Sub",
                            fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        var subImg = new ProductSubImg
                        {
                            SubImg = fileName,
                            ProductId = product.Id
                        };

                        _context.ProductSubImgs.Add(subImg);
                    }
                }

                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DeleteSubImage(int id)
        {
            var subImg = _context.ProductSubImgs.Find(id);

            if (subImg == null)
                return NotFound();

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/img/Product/Sub",
                subImg.SubImg);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _context.ProductSubImgs.Remove(subImg);
            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = subImg.ProductId });
        }
       

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

         
            if (!string.IsNullOrEmpty(product.MainImg))
            {
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/img/Product",
                    product.MainImg);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            var subImgs = _context.ProductSubImgs
                .Where(x => x.ProductId == id)
                .ToList();

            foreach (var sub in subImgs)
            {
                var subPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/img/Product/Sub",
                    sub.SubImg);

                if (System.IO.File.Exists(subPath))
                    System.IO.File.Delete(subPath);
            }

            _context.ProductSubImgs.RemoveRange(subImgs);
            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}