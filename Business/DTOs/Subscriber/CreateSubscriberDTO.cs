using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Subscriber
{
    public class CreateSubscriberDTO
    {
        public Guid Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}
