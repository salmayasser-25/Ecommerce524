using Ecommerce524.Models;
namespace Ecommerce524.ViewModel
{
    public class BrandsVM
    {
        
        public IEnumerable<Brand> Brand { get; set; } = new List<Brand>();
        public double TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
