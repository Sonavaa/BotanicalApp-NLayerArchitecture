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
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; } = default!;
        public decimal? DiscountedPrice { get; set; }
        public string? Description { get; set; }
        public string ProductCode { get; set; } = null!;
        public int Quantity { get; set; }
        public string? ImagePath {  get; set; } 
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsInWishList { get; set; }
        public bool IsInCart { get; set; }
    }
}
