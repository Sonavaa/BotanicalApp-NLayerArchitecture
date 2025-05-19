using Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Subscriber : BaseEntity
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}
