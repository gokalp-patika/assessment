using System;
using System.Collections.Generic;

namespace PhoneDirectory.Domain.Entities
{
    public class Report
    {
        // Constructor for creating new reports
        public Report(string location)
        {
            Id = Guid.NewGuid();
            RequestedDate = DateTime.UtcNow;
            Location = location;
            Status = ReportStatus.InProgress;
        }

        // Constructor for mapping from entity (internal to prevent direct usage)
        internal Report(Guid id, DateTime requestedDate, string location, ReportStatus status, int personCount, int phoneCount)
        {
            Id = id;
            RequestedDate = requestedDate;
            Location = location;
            Status = status;
            PersonCount = personCount;
            PhoneCount = phoneCount;
        }

        public Guid Id { get; private set; }
        public DateTime RequestedDate { get; private set; }
        public ReportStatus Status { get; set; }
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneCount { get; set; }
    }

    public enum ReportStatus
    {
        InProgress,   // "Hazırlanıyor"
        Completed     // "Tamamlandı"
    }
}