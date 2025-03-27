using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhoneDirectory.Domain.Entities;

namespace PhoneDirectory.Domain.Interfaces.Services
{
    public interface IPersonService
    {
        Task<Person> CreatePersonAsync(Person person);
        Task<Person> UpdatePersonAsync(Person person);
        Task DeletePersonAsync(Guid id);
        Task<Person> GetPersonByIdAsync(Guid id);
        Task<IEnumerable<Person>> GetAllPersonsAsync();
        Task<Person> AddContactAsync(Guid personId, Contact contact);
        Task<Person> RemoveContactAsync(Guid personId, Guid contactId);
    }
} 