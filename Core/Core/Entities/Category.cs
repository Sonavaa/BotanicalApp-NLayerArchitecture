using Botanical.Models;
using Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Category : BaseAuditable
    {
        public string Name { get; set; } = null!;
        public string? ImgPath { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
