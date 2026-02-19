using System.ComponentModel;
using Ecommerce524.Models;

namespace Ecommerce524.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string MainImg { get; set; }
        public decimal price { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
        public Categories Category { get; set; } = default!;

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = default!;
        public ICollection<ProductSubImg> SubImages { get; set; } = new List<ProductSubImg>();

    }
}
