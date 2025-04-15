using Botanical.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Products
{
    public class GetProductDTO
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; } = default!;
        public decimal? DiscountedPrice { get; set; }
        public string? Description { get; set; }
        public string ProductCode { get; set; } = null!;
        public int Quantity { get; set; }
        public List<ProductImages>? Images { get; set; }
        public string MainImageUrl => Images?.FirstOrDefault(img => img.IsMain)?.ImgPath ?? "default.jpg";
        public DateTime CreatedAt { get; set; }
    }
}
