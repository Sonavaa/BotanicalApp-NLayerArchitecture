using Core.Entities.Common;

namespace Botanical.Models
{
    public class Product : BaseAuditable
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; } = default!;
        public decimal? DiscountedPrice { get; set; }
        public string? Description { get; set; }
        public string ProductCode { get; set; } = null!;
        public int Quantity { get; set; }
        public List<ProductImages>? Images { get; set; } 
        public string MainImageUrl => Images?.FirstOrDefault(img => img.isMain)?.imgUrl ?? "default.jpg";
    }
}
