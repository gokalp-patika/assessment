using System;
using System.Collections.Generic;

namespace PhoneDirectory.Domain.Entities
{
    public class Person
    {
        // Constructor for creating new persons
        public Person(string firstName, string lastName, string company)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Contacts = new List<Contact>();
        }

        // Constructor for mapping from entity (internal to prevent direct usage)
        internal Person(Guid id, string firstName, string lastName, string company, List<Contact> contacts)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Contacts = contacts;
        }

        public Guid Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public List<Contact> Contacts { get; private set; }
    }
}