namespace Ecommerce524.Models
{
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public List<Product> Products { get; set; }
    }
}
