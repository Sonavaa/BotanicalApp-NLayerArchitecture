using Botanical.Models;
using Core.Entities.Common;
using Core.Entities.Identity;
using System;

namespace Core.Entities
{
    public class Cart : BaseAuditable
    {
        public int Count { get; set; }
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
