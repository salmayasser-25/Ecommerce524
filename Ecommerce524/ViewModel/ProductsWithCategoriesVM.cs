namespace Ecommerce524.ViewModel
{
    
        public class ProductsWithCategoriesVM
        {
            public List<Product> Products { get; set; }
            public List<Categories> Categories { get; set; }
           // public List<Product> RelatedProducts { get; set; } // Add this property to fix CS0117
        }
    
}
