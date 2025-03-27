using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhoneDirectory.Domain.Entities;

namespace PhoneDirectory.Domain.Interfaces.Services
{
    public interface IReportService
    {
        Task<Report> RequestReportAsync(string location);
        Task<Report> GetReportByIdAsync(Guid id);
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report> ProcessReportAsync(Report report);
    }
} 