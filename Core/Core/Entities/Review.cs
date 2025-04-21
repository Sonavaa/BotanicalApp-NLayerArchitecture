using Botanical.Models;
using Core.Entities.Common;
using Core.Entities.Identity;
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
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Range(1, 5)]
        public int? Star { get; set; }
    }
}
