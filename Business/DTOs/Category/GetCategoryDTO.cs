using Botanical.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Category
{
    public class GetCategoryDTO
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Name { get; set; } = null!;
        public string? ImgPath { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
