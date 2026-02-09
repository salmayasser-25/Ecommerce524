namespace Ecommerce524.ViewModel
{
    public class ProductWithRelatedVM
    {
        public Product Product { get; set; } = new Product();
        public List<Product> samePrices { get; set; }
        public List<Product> sameCategories { get; set; } = new List<Product>();
        public List<Product> RelatedProducts { get; set; } = new List<Product>();
       

        

    }
}
