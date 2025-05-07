using AutoMapper;
using Business.DTOs.Contact;
using Business.IServices;
using Core.Entities;
using Core.Repositories;
using Data.Context;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.SecurityTokenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactReadRepository _contactReadRepository;
        private readonly IContactWriteRepository _contactWriteRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ContactService(IContactReadRepository contactReadRepository, IContactWriteRepository contactWriteRepository, AppDbContext context, IMapper mapper)
        {
            _contactReadRepository = contactReadRepository;
            _contactWriteRepository = contactWriteRepository;
            _context = context;
            _mapper = mapper;
        }
        public async Task AddContact(CreateContactDTO addContactDTO)
        {
            bool isContactExist = await _contactReadRepository.GetAll().AnyAsync(c => c.Key == addContactDTO.Key || c.Id == addContactDTO.Id);
            if (isContactExist)
            {
                throw new BadRequestException("This Contact already exists.");
            }

            var contact = _mapper.Map<Contact>(addContactDTO);

            contact.Id = Guid.NewGuid();

            await _contactWriteRepository.AddAsync(contact);
            await _contactWriteRepository.SaveChangeAsync();
        }

        public async Task<List<GetContactDTO>> GetAllContactsAsync()
        {
            var contacts = await _contactReadRepository.GetAll()
           .Select(c => new GetContactDTO
           {
               Id = c.Id,
               Key = c.Key,
               Values = c.Values,
               IsDeleted = c.IsDeleted,
           })
           .ToListAsync();

            foreach (var contact in contacts)
            {
                contact.Values = contact.Values?.ToList();
            }


            return contacts;
        }

        public async Task<GetContactDTO> GetContactById(Guid id)
        {
            var contact = await _contactReadRepository.GetAll()
                .Where(c => !c.IsDeleted && c.Id == id)
                .Select(c => new GetContactDTO
                {
                    Id = c.Id,
                    Key = c.Key,
                    Values = c.Values,
                    IsDeleted = c.IsDeleted,
                })
                .FirstOrDefaultAsync();

            return contact;
        }

        public async Task DeleteContact(Guid Id)
        {
            var contact = await _contactReadRepository.GetByIdAsync(Id.ToString());
            if (contact == null)
            {
                throw new Exception("Contact not found");
            }
            contact.SoftDelete();

            _contactWriteRepository.Update(contact);
            await _contactWriteRepository.SaveChangeAsync();
        }

        public async Task RestoreContact(Guid Id)
        {
            var contact = await _contactReadRepository.GetByIdAsync(Id.ToString());
            if (contact == null)
            {
                throw new Exception("Contact not found");
            }
            contact.Restore();

            _contactWriteRepository.Update(contact);
            await _contactWriteRepository.SaveChangeAsync();
        }

        public async Task EditContact(Guid Id, CreateContactDTO updateContactDTO)
        {
            var contact = await _contactReadRepository.GetByIdAsync(Id.ToString());
            if (contact == null)
            {
                throw new Exception("Contact not found");
            }

            contact.Key = updateContactDTO.Key;
            contact.Values = updateContactDTO.Values;
            contact.UpdatedAt = DateTime.UtcNow.AddHours(4);
            contact.UpdatedBy = "Admin";

            _contactWriteRepository.Update(contact);
            await _contactWriteRepository.SaveChangeAsync();
        }

    }
}
