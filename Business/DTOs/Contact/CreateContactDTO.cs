using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Contact
{
    public class CreateContactDTO
    {
        public string Key { get; set; } = null!;
        public List<string> Values { get; set; } = new List<string>();
        public Guid Id { get; set; }
    }
}
