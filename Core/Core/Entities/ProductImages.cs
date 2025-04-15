using Core.Entities.Common;

namespace Botanical.Models
{
    public class ProductImages : BaseAuditable
    {
        public string? ImgPath { get; set; }
        public bool IsMain { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
