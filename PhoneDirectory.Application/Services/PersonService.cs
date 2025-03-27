using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Domain.Interfaces;
using PhoneDirectory.Domain.Interfaces.Services;

namespace PhoneDirectory.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<Person> CreatePersonAsync(Person person)
        {
            return await Task.FromResult(_personRepository.Add(person));
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            return await Task.FromResult(_personRepository.Update(person));
        }

        public async Task DeletePersonAsync(Guid id)
        {
            await Task.Run(() => _personRepository.Delete(id));
        }

        public async Task<Person> GetPersonByIdAsync(Guid id)
        {
            return await Task.FromResult(_personRepository.GetById(id));
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            return await Task.FromResult(_personRepository.GetAll());
        }

        public async Task<Person> AddContactAsync(Guid personId, Contact contact)
        {
            var person = _personRepository.GetById(personId);
            contact.PersonId = personId;
            person.Contacts.Add(contact);
            return await Task.FromResult(_personRepository.Update(person));
        }

        public async Task<Person> RemoveContactAsync(Guid personId, Guid contactId)
        {
            var person = _personRepository.GetById(personId);
            person.Contacts.RemoveAll(c => c.Id == contactId);
            return await Task.FromResult(_personRepository.Update(person));
        }

        // Implement other interface methods...
    }
} 