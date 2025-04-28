using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Tag
{
    public class GetTagsDTO
    {
        public Guid Id { get; set; }
        public string Name  { get; set; }
        public bool IsDeleted { get; set; }
    }
}
