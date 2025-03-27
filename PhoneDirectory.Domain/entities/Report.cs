using System;
using System.Collections.Generic;

namespace PhoneDirectory.Domain.Entities
{
    public class Report
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime RequestedDate { get; set; } = DateTime.UtcNow;
        public ReportStatus Status { get; set; }
        public required string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneCount { get; set; }
    }

    public enum ReportStatus
    {
        InProgress,   // "Hazırlanıyor"
        Completed     // "Tamamlandı"
    }
}