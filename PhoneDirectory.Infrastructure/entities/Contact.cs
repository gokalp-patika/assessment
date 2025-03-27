using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PhoneDirectory.Infrastructure.Entities
{
    [Table("Contacts")]
    public class ContactEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        
        // Stored as string for persistence; mapping to Domain.ContactType will occur in the repository
        [Required]
        [StringLength(50)]
        public required string ContactType { get; set; }

        [Required]
        [StringLength(200)]
        public required string Content { get; set; }
        
        [ForeignKey("PersonId")]
        public required PersonEntity Person { get; set; }
    }
}