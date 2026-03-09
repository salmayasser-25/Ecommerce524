using Ecommerce524.Models;
using Ecommerce524.Repositories;
using Ecommerce524.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce524.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<Categories> _categoryRepo;
        private readonly IRepository<Brand> _brandRepo;
        private readonly IRepository<ProductSubImg> _subImgRepo;

        public ProductService(
            IRepository<Product> productRepo,
            IRepository<Categories> categoryRepo,
            IRepository<Brand> brandRepo,
            IRepository<ProductSubImg> subImgRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _brandRepo = brandRepo;
            _subImgRepo = subImgRepo;
        }

        // ===================== GET PRODUCTS WITH FILTERS & PAGINATION =====================
        public async Task<ProductsVM> GetProductsAsync(ProductFilterVM filter, int page)
        {
            var products = await _productRepo.GetAsync(
                filter: null,
                tracked: false,
                e => e.Category,
                e => e.Brand
            );

            var query = products.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(e => e.Name.Contains(filter.Name));

            if (filter.MinPrice.HasValue)
                query = query.Where(e => e.price >= filter.MinPrice);

            if (filter.MaxPrice.HasValue)
                query = query.Where(e => e.price <= filter.MaxPrice);

            if (filter.CategoryId.HasValue)
                query = query.Where(e => e.CategoryId == filter.CategoryId);

            if (filter.BrandId.HasValue)
                query = query.Where(e => e.BrandId == filter.BrandId);

            if (filter.LessQuantity)
                query = query.Where(e => e.Quantity < 50);

            int pageSize = 5;
            var totalPages = Math.Ceiling(query.Count() / (double)pageSize);

            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var categories = await _categoryRepo.GetAsync(null, false);
            var brands = await _brandRepo.GetAsync(null, false);

            return new ProductsVM
            {
                Products = data,
                Categories = categories,
                Brands = brands,
                CurrentPage = page,
                TotalPages = totalPages,
                productFilterVM = filter
            };
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _productRepo.GetOneAsync(
                e => e.Id == id,
                tracked: true,
                e => e.Category,
                e => e.Brand,
                e => e.SubImages // 👈 ضروري عشان يظهروا في الـ Edit View
            );
        }

        public async Task<List<Categories>> GetCategoriesAsync()
        {
            return await _categoryRepo.GetAsync(null, false);
        }

        public async Task<List<Brand>> GetBrandsAsync()
        {
            return await _brandRepo.GetAsync(null, false);
        }

        // ===================== CREATE =====================
        public async Task CreateAsync(Product product, IFormFile MainImg, List<IFormFile>? SubImgs)
        {
            if (MainImg != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Product", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await MainImg.CopyToAsync(stream);

                product.MainImg = fileName;
            }

            await _productRepo.CreateAsync(product);
            await _productRepo.SaveChangesAsync();

            if (SubImgs != null && SubImgs.Any())
            {
                foreach (var subImg in SubImgs)
                {
                    var subFileName = Guid.NewGuid() + Path.GetExtension(subImg.FileName);
                    var subPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Product/Sub", subFileName);

                    using var stream = new FileStream(subPath, FileMode.Create);
                    await subImg.CopyToAsync(stream);

                    await _subImgRepo.CreateAsync(new ProductSubImg
                    {
                        ProductId = product.Id,
                        SubImg = subFileName
                    });
                }
                await _subImgRepo.SaveChangesAsync();
            }
        }

        // ===================== UPDATE =====================
        public async Task UpdateAsync(Product product, IFormFile? MainImg, List<IFormFile>? SubImgs)
        {
            var existingProduct = await _productRepo.GetByIdAsync(product.Id);
            if (existingProduct == null) return;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.price = product.price;
            existingProduct.Discount = product.Discount;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Rate = product.Rate;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.BrandId = product.BrandId;
            existingProduct.Status = product.Status;

            if (MainImg != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Product", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await MainImg.CopyToAsync(stream);

                existingProduct.MainImg = fileName;
            }

            _productRepo.Update(existingProduct);
            await _productRepo.SaveChangesAsync();

            if (SubImgs != null && SubImgs.Any())
            {
                foreach (var subImg in SubImgs)
                {
                    var subFileName = Guid.NewGuid() + Path.GetExtension(subImg.FileName);
                    var subPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Product/Sub", subFileName);

                    using var stream = new FileStream(subPath, FileMode.Create);
                    await subImg.CopyToAsync(stream);

                    await _subImgRepo.CreateAsync(new ProductSubImg
                    {
                        ProductId = product.Id,
                        SubImg = subFileName
                    });
                }
                await _subImgRepo.SaveChangesAsync();
            }
        }

        // ===================== DELETE =====================
        public async Task DeleteAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product != null)
            {
                _productRepo.Delete(product);
                await _productRepo.SaveChangesAsync();
            }
        }

        public async Task<int> DeleteSubImageAsync(int id)
        {
            var subImg = await _subImgRepo.GetByIdAsync(id);
            if (subImg == null) return 0;

            int productId = subImg.ProductId;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Product/Sub", subImg.SubImg);
            if (File.Exists(path)) File.Delete(path);

            _subImgRepo.Delete(subImg);
            await _subImgRepo.SaveChangesAsync();

            return productId;
        }
    }
}