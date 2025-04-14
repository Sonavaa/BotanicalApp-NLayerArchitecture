using Core.Entities.Common;

namespace Botanical.Models
{
    public class ProductImages : BaseAuditable
    {
        public string? imgUrl { get; set; }
        public bool isMain { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
