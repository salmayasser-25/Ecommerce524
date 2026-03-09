using Microsoft.AspNetCore.Mvc;
using Ecommerce524.Services;
using Ecommerce524.Models;

namespace Ecommerce524.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? name, int page = 1)
        {
            var categories = await _categoryService.GetCategoriesAsync(name);

            if (page < 1)
                page = 1;

            int pageSize = 5;

            var totalPages = Math.Ceiling(categories.Count / (double)pageSize);

            var data = categories
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return View(new CategoriesVM
            {
                Category = data,
                TotalPages = totalPages,
                CurrentPage = page
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categories category)
        {
            await _categoryService.CreateAsync(category);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Categories category)
        {
            await _categoryService.UpdateAsync(category);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}