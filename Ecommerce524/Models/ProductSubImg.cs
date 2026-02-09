namespace Ecommerce524.Models
{
    public class ProductSubImg
    {
        public int Id { get; set; }
        public string SubImg { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

    }
}
