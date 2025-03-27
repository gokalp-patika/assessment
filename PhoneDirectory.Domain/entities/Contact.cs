using System;
using System.Collections.Generic;

namespace PhoneDirectory.Domain.Entities
{
    public class Contact
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PersonId { get; set; }
        public ContactType Type { get; set; }
        public required string Content { get; set; }
    }

    public enum ContactType
    {
        Phone,
        Email,
        Location
    }
}