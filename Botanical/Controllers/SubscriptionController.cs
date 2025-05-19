using Business.DTOs.Subscriber;
using Business.IServices;
using Core.Entities;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Botanical.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ISubscriberReadRepository _SubscriberReadRepository;
        private readonly ISubscriberWriteRepository _SubscriberWriteRepository;
        private readonly ISubscriptionService _SubscriptionService;
        public SubscriptionController(ISubscriberReadRepository subscriberReadRepository, ISubscriberWriteRepository subscriberWriteRepository, ISubscriptionService subscriptionService)
        {
            _SubscriberReadRepository = subscriberReadRepository;
            _SubscriberWriteRepository = subscriberWriteRepository;
            _SubscriptionService = subscriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(CreateSubscriberDTO subscribe)
        {
            if (!ModelState.IsValid)
            {
                TempData["SubscriptionError"] = "Invalid email address.";
                return RedirectToAction("Index", "Home");
            }

            if (await _SubscriberReadRepository.ExistsAsync(subscribe.Email))
            {
                TempData["SubscriptionError"] = "You are already subscribed.";
                return RedirectToAction("Index", "Home");
            }

            if (!User.Identity.IsAuthenticated)
            {
                TempData["SubscriptionError"] = "Please log in to subscribe to the newsletter.";
                return RedirectToAction("Login", "User");
            }
            if (await _SubscriberReadRepository.ExistsAsync(subscribe.Email))
            {
                TempData["SubscriptionError"] = "You already subscript with this email.";
                return RedirectToAction("Index", "Home");
            }

            var subscriber = new Subscriber
            {
                Id = Guid.NewGuid(),
                Email = subscribe.Email,
                IsDeleted = false
            };

            await _SubscriberWriteRepository.AddAsync(subscriber);
            await _SubscriberWriteRepository.SaveChangeAsync();

            try
            {
                await _SubscriptionService.SendSubscriptionConfirmationAsync(subscribe.Email);
                TempData["SubscriptionSuccess"] = "Thank you for subscribing!";
            }
            catch (Exception ex)
            {
                TempData["SubscriptionError"] = "Failed to send confirmation email. Please try again later.";
                // optional: log ex.Message
            }

            return RedirectToAction("Index", "Home");
        }


        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
