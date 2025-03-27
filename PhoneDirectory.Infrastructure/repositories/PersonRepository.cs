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
    public class PersonRepository : IPersonRepository
    {
        private readonly PhoneDirectoryDbContext _dbContext;

        public PersonRepository(PhoneDirectoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Person> GetByIdAsync(Guid id)
        {
            var entity = await _dbContext.Persons.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Person with ID {id} not found");
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map person");
        }

        public Person Add(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            
            var entity = person.ToEntity() ?? throw new InvalidOperationException("Failed to map person to entity");
            _dbContext.Persons.Add(entity);
            _dbContext.SaveChanges();
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map created person");
        }

        public Person Update(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            
            var entity = person.ToEntity() ?? throw new InvalidOperationException("Failed to map person to entity");
            _dbContext.Persons.Update(entity);
            _dbContext.SaveChanges();
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map updated person");
        }

        public void Delete(Guid id)
        {
            var entity = _dbContext.Persons.Find(id);
            if (entity != null)
            {
                _dbContext.Persons.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public Person GetById(Guid id)
        {
            var entity = _dbContext.Persons
                .Include(p => p.Contacts)
                .FirstOrDefault(p => p.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Person with ID {id} not found");
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map person");
        }

        public IEnumerable<Person> GetAll()
        {
            return _dbContext.Persons
                .Include(p => p.Contacts)
                .AsNoTracking()
                .ToList()
                .Select(e => e.ToDomain() ?? throw new InvalidOperationException("Failed to map person"));
        }
    }
}