using Botanical.Models;
using Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Cart : BaseAuditable
    {
        public int Count { get; set; }
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
        public Guid? UserId { get; set; }
        public AppUser? User { get; set; }
    }
}
