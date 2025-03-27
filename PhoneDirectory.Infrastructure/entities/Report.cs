using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PhoneDirectory.Infrastructure.Entities
{
    [Table("Reports")]
    public class ReportEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime RequestedDate { get; set; }
        
        // Stored as string; will be mapped to Domain.ReportStatus
        [Required]
        [StringLength(50)]
        public required string Status { get; set; }

        [Required]
        [StringLength(200)]
        public required string Location { get; set; }

        [Required]
        public int PersonCount { get; set; }

        [Required]
        public int PhoneCount { get; set; }
    }
}