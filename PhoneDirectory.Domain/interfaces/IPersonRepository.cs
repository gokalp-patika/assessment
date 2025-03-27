using System;
using System.Collections.Generic;
using PhoneDirectory.Domain.Entities;

namespace PhoneDirectory.Domain.Interfaces
{
    public interface IPersonRepository
    {
        Person Add(Person person);
        Person Update(Person person);
        void Delete(Guid id);
        Person GetById(Guid id);
        IEnumerable<Person> GetAll();
    }
}