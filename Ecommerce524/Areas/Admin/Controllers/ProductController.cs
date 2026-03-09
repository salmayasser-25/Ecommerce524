using Ecommerce524.Services;
using Ecommerce524.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce524.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ===================== INDEX =====================
        public async Task<IActionResult> Index(ProductFilterVM filter, int page = 1)
        {
            var data = await _productService.GetProductsAsync(filter, page);

            return View(data);
        }

        // ===================== CREATE =====================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.Brands = await _productService.GetBrandsAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile MainImg, List<IFormFile>? SubImgs)
        {
            await _productService.CreateAsync(product, MainImg, SubImgs);

            return RedirectToAction(nameof(Index));
        }

        // ===================== EDIT =====================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.Brands = await _productService.GetBrandsAsync();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile? MainImg, List<IFormFile>? SubImgs)
        {
            await _productService.UpdateAsync(product, MainImg, SubImgs);

            return RedirectToAction(nameof(Index));
        }

        // ===================== DELETE =====================
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        // ===================== DELETE SUB IMAGE =====================
        public async Task<IActionResult> DeleteSubImage(int id)
        {
            var productId = await _productService.DeleteSubImageAsync(id);

            return RedirectToAction("Edit", new { id = productId });
        }
    }
}