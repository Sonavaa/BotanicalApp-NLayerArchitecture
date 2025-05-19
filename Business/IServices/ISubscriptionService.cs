using Business.DTOs.Contact;
using Business.DTOs.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface ISubscriptionService
    {
        Task<List<GetSubscriberDTO>> GetAllSubscribersAsync();
        Task<GetSubscriberDTO> GetSubscriberByEmail(string email, Guid Id);
        Task SendSubscriptionConfirmationAsync(string toEmail);
    }
}
