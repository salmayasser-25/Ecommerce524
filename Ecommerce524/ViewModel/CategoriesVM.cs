using Ecommerce524.Models;
namespace Ecommerce524.ViewModel
{
    public class CategoriesVM
    {
        public IEnumerable<Categories> Category{ get; set; }
        public double TotalPages { get; set; }
        public int CurrentPage { get; set; }

    }
}
