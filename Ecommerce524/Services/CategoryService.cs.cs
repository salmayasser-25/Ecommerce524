using Ecommerce524.Models;
using Ecommerce524.Repositories;

namespace Ecommerce524.Services
{
    public class CategoryService
    {
        private readonly IRepository<Categories> _categoryRepo;

        public CategoryService(IRepository<Categories> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<List<Categories>> GetCategoriesAsync(string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return await _categoryRepo.GetAsync(e => e.Name.Contains(name), false);
            }

            return await _categoryRepo.GetAsync(null, false);
        }

        public async Task<Categories?> GetByIdAsync(int id)
        {
            return await _categoryRepo.GetByIdAsync(id);
        }

        public async Task CreateAsync(Categories category)
        {
            await _categoryRepo.CreateAsync(category);
            await _categoryRepo.SaveChangesAsync();
        }

        public async Task UpdateAsync(Categories category)
        {
            _categoryRepo.Update(category);
            await _categoryRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category != null)
            {
                _categoryRepo.Delete(category);
                await _categoryRepo.SaveChangesAsync();
            }
        }
    }
}