using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Tag
{
    public class GetProductTagDTO
    {
        public string Name { get; set; }
        public Guid TagId { get; set; }
        public List<Guid> ProductsIds { get; set; } = new List<Guid>();
    }
}
