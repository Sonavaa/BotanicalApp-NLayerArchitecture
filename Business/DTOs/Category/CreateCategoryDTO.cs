using Botanical.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Category
{
    public class CreateCategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImgPath { get; set; }
        public IEnumerable<Product>? Products { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
