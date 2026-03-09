using Ecommerce524.Models;

namespace Ecommerce524.Services
{
    public interface ICategoryService
    {
        Task<List<Categories>> GetCategoriesAsync(string? name);

        Task<Categories?> GetByIdAsync(int id);

        Task CreateAsync(Categories category);

        Task UpdateAsync(Categories category);

        Task DeleteAsync(int id);
    }
}