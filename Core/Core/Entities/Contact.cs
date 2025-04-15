using Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Contact : BaseAuditable
    {
        public string Key { get; set; } = null!;
        public IEnumerable<string>? Value { get; set; }
    }
}
