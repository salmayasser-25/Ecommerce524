using Ecommerce524.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce524.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index(string? name, int page = 1)
        {
            var result =
                await _brandService.GetFilteredBrandsAsync(name, page, 5);

            return View(new BrandsVM
            {
                Brand = result.Brands,
                TotalPages = result.TotalPages,
                CurrentPage = page
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand, IFormFile? Logo)
        {
            await _brandService.CreateBrandAsync(brand, Logo);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Brand brand, IFormFile? Logo)
        {
            await _brandService.UpdateBrandAsync(brand, Logo);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.DeleteBrandAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}