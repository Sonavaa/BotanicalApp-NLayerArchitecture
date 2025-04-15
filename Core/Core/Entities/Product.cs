using Core.Entities;
using Core.Entities.Common;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Botanical.Models
{
    public class Product : BaseAuditable
    {
        public string Name { get; set; } = null!;
        public Category CategoryId { get; set; } = null!;
        public Category? Category { get; set; }
        public decimal Price { get; set; } = default!;
        public decimal? DiscountedPrice { get; set; }
        public string? Description { get; set; }
        public string ProductCode { get; set; } = null!;
        public int Quantity { get; set; }
        public bool? IsInWishList { get; set; }
        public IEnumerable<ProductImages>? Images { get; set; } 
        public string MainImageUrl => Images?.FirstOrDefault(img => img.IsMain)?.ImgPath ?? "default.jpg";
        public IEnumerable<ProductTag>? ProductTags { get; set; }
        public IEnumerable<Review>? Reviews { get; set; }

        [NotMapped]
        public IEnumerable<Guid>? TagIds { get; set; }

        [NotMapped]
        public IEnumerable<IFormFile>? ImgFile { get; set; }
    }
}
