using Ecommerce524.Models;

namespace Ecommerce524.Services
{
    public interface IBrandService
    {
        Task<(List<Brand> Brands, double TotalPages)>
            GetFilteredBrandsAsync(string? name, int page, int pageSize);

        Task<Brand?> GetBrandByIdAsync(int id);

        Task CreateBrandAsync(Brand brand, IFormFile? logo);

        Task UpdateBrandAsync(Brand brand, IFormFile? logo);

        Task DeleteBrandAsync(int id);
    }
}