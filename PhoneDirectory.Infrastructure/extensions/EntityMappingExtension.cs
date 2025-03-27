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
            if (entity == null) return null;

            // Create a new person with required properties
            var person = new Person(
                firstName: entity.FirstName,
                lastName: entity.LastName,
                company: entity.Company
            );

            // Use reflection to set the Id since it's private
            typeof(Person).GetProperty("Id")?.SetValue(person, entity.Id);

            // Add contacts if any exist
            if (entity.Contacts != null)
            {
                foreach (var contact in entity.Contacts)
                {
                    var domainContact = contact.ToDomain();
                    if (domainContact != null)
                    {
                        person.Contacts.Add(domainContact);
                    }
                }
            }

            return person;
        }

        public static PersonEntity? ToEntity(this Person? domain)
        {
            if (domain == null) return null;
            
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

            // Create a new report with the location
            var report = new Report(location: entity.Location);

            // Use reflection to set the private properties
            typeof(Report).GetProperty("Id")?.SetValue(report, entity.Id);
            typeof(Report).GetProperty("RequestedDate")?.SetValue(report, entity.RequestedDate);

            // Set the public properties
            report.Status = Enum.Parse<ReportStatus>(entity.Status);
            report.PersonCount = entity.PersonCount;
            report.PhoneCount = entity.PhoneCount;

            return report;
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