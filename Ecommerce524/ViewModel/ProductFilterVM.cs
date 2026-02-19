namespace Ecommerce524.ViewModel
{
    public class ProductFilterVM
    {
        public string? Name { get; set; }
        public long? MinPrice { get; set; }
        public long? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public bool LessQuantity { get; set; }

    }
}
