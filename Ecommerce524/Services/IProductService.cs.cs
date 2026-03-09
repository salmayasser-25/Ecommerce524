using Ecommerce524.Models;
using Ecommerce524.ViewModel;

namespace Ecommerce524.Services
{
    public interface IProductService
    {
        Task<ProductsVM> GetProductsAsync(ProductFilterVM filter, int page);

        Task<Product?> GetByIdAsync(int id);

        Task<List<Categories>> GetCategoriesAsync();

        Task<List<Brand>> GetBrandsAsync();

        Task CreateAsync(Product product, IFormFile MainImg, List<IFormFile>? SubImgs);

        Task UpdateAsync(Product product, IFormFile? MainImg, List<IFormFile>? SubImgs);

        Task DeleteAsync(int id);

        Task<int> DeleteSubImageAsync(int id);
    }
}