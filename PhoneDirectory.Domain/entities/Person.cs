using System;
using System.Collections.Generic;

namespace PhoneDirectory.Domain.Entities
{
    public class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Company { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();
    }
}