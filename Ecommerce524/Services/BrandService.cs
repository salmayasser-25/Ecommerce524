using Ecommerce524.Models;
using Ecommerce524.Repositories;
using System.Linq.Expressions;

namespace Ecommerce524.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _brandRepo;

        public BrandService(IRepository<Brand> brandRepo)
        {
            _brandRepo = brandRepo;
        }

        public async Task<(List<Brand> Brands, double TotalPages)>
            GetFilteredBrandsAsync(string? name, int page, int pageSize)
        {
            Expression<Func<Brand, bool>>? filter = null;

            if (!string.IsNullOrEmpty(name))
                filter = b => b.Name.Contains(name);

            var brands = await _brandRepo.GetAsync(filter, tracked: false);

            double totalPages =
                Math.Ceiling(brands.Count / (double)pageSize);

            var pagedBrands = brands
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (pagedBrands, totalPages);
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _brandRepo.GetByIdAsync(id);
        }

        public async Task CreateBrandAsync(Brand brand, IFormFile? logo)
        {
            if (logo != null)
                brand.Logo = await SaveLogoAsync(logo);

            await _brandRepo.CreateAsync(brand);
            await _brandRepo.SaveChangesAsync();
        }

        public async Task UpdateBrandAsync(Brand brand, IFormFile? logo)
        {
            var oldBrand = await _brandRepo.GetByIdAsync(brand.Id);
            if (oldBrand == null)
                throw new Exception("Brand not found");

            if (logo != null)
            {
                if (!string.IsNullOrEmpty(oldBrand.Logo))
                    DeleteLogoFile(oldBrand.Logo);

                brand.Logo = await SaveLogoAsync(logo);
            }
            else
            {
                brand.Logo = oldBrand.Logo;
            }

            var existingBrand = await _brandRepo.GetByIdAsync(brand.Id);

            if (existingBrand != null)
            {
                existingBrand.Name = brand.Name;
                existingBrand.Status = brand.Status;

                if (logo != null)
                {
                    // upload logic
                    existingBrand.Logo = brand.Logo;
                }

                _brandRepo.Update(existingBrand);
            }
            await _brandRepo.SaveChangesAsync();
        }

        public async Task DeleteBrandAsync(int id)
        {
            var brand = await _brandRepo.GetByIdAsync(id);
            if (brand == null)
                throw new Exception("Brand not found");

            if (!string.IsNullOrEmpty(brand.Logo))
                DeleteLogoFile(brand.Logo);

            _brandRepo.Delete(brand);
            await _brandRepo.SaveChangesAsync();
        }

        private async Task<string> SaveLogoAsync(IFormFile logo)
        {
            var newFileName =
                Guid.NewGuid().ToString() +
                DateTime.UtcNow.ToString("yyyyMMddHHmmss") +
                Path.GetExtension(logo.FileName);

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/img/brand-logo",
                newFileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await logo.CopyToAsync(stream);
            }

            return newFileName;
        }

        private void DeleteLogoFile(string fileName)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/img/brand-logo",
                fileName);

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}