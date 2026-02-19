namespace Ecommerce524.ViewModel
{
    public class ProductsVM
    { 
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Categories> Categories { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public double TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public ProductFilterVM productFilterVM { get; set; }
    }
}
