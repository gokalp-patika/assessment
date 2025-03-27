using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Infrastructure.Entities;

namespace PhoneDirectory.Infrastructure.Extensions
{
    public static class EntityMappingExtension
    {
        public static Contact? ToDomain(this ContactEntity? entity)
        {
            if (entity == null) return null;
            
            return new Contact
            {
                Id = entity.Id,
                PersonId = entity.PersonId,
                Type = Enum.Parse<ContactType>(entity.ContactType),
                Content = entity.Content
            };
        }

        public static ContactEntity? ToEntity(this Contact? domain)
        {
            if (domain == null) return null;
            
            var entity = new ContactEntity
            {
                Id = domain.Id,
                PersonId = domain.PersonId,
                ContactType = domain.Type.ToString(),
                Content = domain.Content,
                Person = null! // We'll let EF Core handle this relationship
            };
            return entity;
        }

        public static Person? ToDomain(this PersonEntity? entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            return new Person
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Company = entity.Company,
                Contacts = entity.Contacts
                    .Select(c => c.ToDomain() ?? throw new InvalidOperationException("Invalid contact mapping"))
                    .ToList()
            };
        }

        public static PersonEntity? ToEntity(this Person? domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            
            return new PersonEntity
            {
                Id = domain.Id,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                Company = domain.Company,
                Contacts = domain.Contacts
                    .Select(c => c.ToEntity() ?? throw new InvalidOperationException("Invalid contact mapping"))
                    .ToList()
            };
        }

        public static Report? ToDomain(this ReportEntity? entity)
        {
            if (entity == null) return null;
            
            return new Report
            {
                Id = entity.Id,
                RequestedDate = entity.RequestedDate,
                Status = Enum.Parse<ReportStatus>(entity.Status),
                Location = entity.Location,
                PersonCount = entity.PersonCount,
                PhoneCount = entity.PhoneCount
            };
        }

        public static ReportEntity? ToEntity(this Report? domain)
        {
            if (domain == null) return null;
            
            return new ReportEntity
            {
                Id = domain.Id,
                RequestedDate = domain.RequestedDate,
                Status = domain.Status.ToString(),
                Location = domain.Location,
                PersonCount = domain.PersonCount,
                PhoneCount = domain.PhoneCount
            };
        }
    }
} 