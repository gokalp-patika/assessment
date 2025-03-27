using System;
using System.Collections.Generic;
using PhoneDirectory.Domain.Entities;

namespace PhoneDirectory.Domain.Interfaces
{
    public interface IContactRepository
    {
        Contact Add(Contact contact);
        Contact Update(Contact contact);
        void Delete(Guid id);
        Contact GetById(Guid id);
        IEnumerable<Contact> GetAll();
    }
}