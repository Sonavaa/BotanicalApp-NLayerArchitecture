using Business.DTOs.Subscriber;
using Business.IServices;
using Core.Repositories;
using MailKit.Net.Smtp;
using MimeKit;


namespace Business.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriberReadRepository _subscriberReadRepository;

        public SubscriptionService(ISubscriberReadRepository subscriberReadRepository)
        {
            _subscriberReadRepository = subscriberReadRepository;
        }

        public async Task<List<GetSubscriberDTO>> GetAllSubscribersAsync()
        {
            var subscribers = _subscriberReadRepository.GetAll();

            return subscribers.Select(s => new GetSubscriberDTO
            {
                Id = s.Id,
                Email = s.Email
            }).ToList();
        }

        public async Task<GetSubscriberDTO> GetSubscriberByEmail(string email, Guid id)
        {
            var subscriber = _subscriberReadRepository
                .GetAll()
                .FirstOrDefault(s => s.Email == email && s.Id == id);

            if (subscriber == null)
                return null;

            return new GetSubscriberDTO
            {
                Id = subscriber.Id,
                Email = subscriber.Email
            };
        }

        public async Task SendSubscriptionConfirmationAsync(string toEmail)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("sonaabbasovx@gmail.com"));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = "Newsletter Subscription";
                email.Body = new TextPart("plain")
                {
                    Text = "Thank you for subscribing to our newsletter!"
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("sonaabbasovx@gmail.com", "xxuv ltkh trsw oxet");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
