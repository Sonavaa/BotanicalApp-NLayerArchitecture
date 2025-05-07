using Business.DTOs.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IContactService
    {
        Task AddContact(CreateContactDTO addContactDTO);
        Task<List<GetContactDTO>> GetAllContactsAsync();
        Task<GetContactDTO> GetContactById(Guid Id);
        Task EditContact(Guid Id, CreateContactDTO updateContactDTO);
        Task DeleteContact(Guid Id);
        Task RestoreContact(Guid Id);
    }
}
