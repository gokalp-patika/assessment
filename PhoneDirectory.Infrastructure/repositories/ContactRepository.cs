using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Domain.Interfaces;
using PhoneDirectory.Infrastructure.Entities;
using PhoneDirectory.Infrastructure.Extensions;

namespace PhoneDirectory.Infrastructure.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly PhoneDirectoryDbContext _dbContext;

        public ContactRepository(PhoneDirectoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Contact> GetByIdAsync(Guid id)
        {
            var entity = await _dbContext.Contacts.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Contact with ID {id} not found");
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map contact");
        }

        public Contact Add(Contact contact)
        {
            if (contact == null) throw new ArgumentNullException(nameof(contact));
            
            var entity = contact.ToEntity() ?? throw new InvalidOperationException("Failed to map contact to entity");
            _dbContext.Contacts.Add(entity);
            _dbContext.SaveChanges();
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map created contact");
        }

        public Contact Update(Contact contact)
        {
            if (contact == null) throw new ArgumentNullException(nameof(contact));
            
            var entity = contact.ToEntity();
            if (entity == null) throw new InvalidOperationException("Failed to map contact to entity");
            
            _dbContext.Contacts.Update(entity);
            _dbContext.SaveChanges();
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map updated contact");
        }

        public void Delete(Guid id)
        {
            var entity = _dbContext.Contacts.Find(id);
            if (entity != null)
            {
                _dbContext.Contacts.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public Contact GetById(Guid id)
        {
            var entity = _dbContext.Contacts.FirstOrDefault(c => c.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Contact with ID {id} not found");
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map contact");
        }

        public IEnumerable<Contact> GetAll()
        {
            return _dbContext.Contacts
                .AsNoTracking()
                .ToList()
                .Select(e => e.ToDomain() ?? throw new InvalidOperationException("Failed to map contact"));
        }
    }
}