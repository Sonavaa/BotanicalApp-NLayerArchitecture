using AutoMapper;
using Business.DTOs.Contact;
using Business.DTOs.Contact;
using Business.DTOs.Contact;
using Business.IServices;
using Data.Context;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Botanical.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ContactController(IContactService contactService, AppDbContext context, IMapper mapper)
        {
            _contactService = contactService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return View(contacts);
        }

        [HttpGet]
        public async Task<IActionResult> AddContact()
        {
            var model = new CreateContactDTO
            {
                Values = new List<string>() 
            };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContact(CreateContactDTO addContactDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(addContactDTO);
            }

            try
            {
                await _contactService.AddContact(addContactDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(addContactDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContact(Guid Id)
        {
            try
            {
                await _contactService.DeleteContact(Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreContact(Guid Id)
        {
            try
            {
                await _contactService.RestoreContact(Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditContact(Guid Id)
        {
            try
            {
                var Contact = await _contactService.GetContactById(Id);

                if (Contact == null)
                {
                    return NotFound("Contact not found.");
                }

                var updateContactDTO = new CreateContactDTO
                {
                    Id = Contact.Id,
                    Key = Contact.Key,
                    Values = Contact.Values.ToList()
                };

                return View(updateContactDTO);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if necessary
                return View("EditContact", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContact(Guid id, CreateContactDTO updateContactDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(updateContactDTO);
            }

            await _contactService.EditContact(id, updateContactDTO);
            TempData["Success"] = "Contact updated successfully!";
            return RedirectToAction("Index");

        }
    }
}
