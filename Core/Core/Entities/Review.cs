using Botanical.Models;
using Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Review : BaseAuditable
    {
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
        public Guid? UserId { get; set; }
        public AppUser? User { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Range(1, 5)]
        public int? Star { get; set; }
    }
}
