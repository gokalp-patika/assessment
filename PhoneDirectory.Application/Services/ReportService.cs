using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Domain.Interfaces;
using PhoneDirectory.Domain.Interfaces.Message;
using PhoneDirectory.Domain.Interfaces.Services;
using PhoneDirectory.Domain.Models;
using System.Linq;

namespace PhoneDirectory.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IKafkaProducer _kafkaProducer;

        public ReportService(
            IReportRepository reportRepository,
            IPersonRepository personRepository,
            IKafkaProducer kafkaProducer)
        {
            _reportRepository = reportRepository;
            _personRepository = personRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<Report> RequestReportAsync(string location)
        {
            var report = new Report(location);

            var savedReport = _reportRepository.Add(report);
            await _kafkaProducer.PublishReportRequestAsync(new ReportRequest { ReportId = savedReport.Id });
            return savedReport;
        }

        public async Task<Report> GetReportByIdAsync(Guid id)
        {
            return await Task.FromResult(_reportRepository.GetById(id));
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await Task.FromResult(_reportRepository.GetAll());
        }

        public async Task<Report> ProcessReportAsync(Report report)
        {
            var allPersons = _personRepository.GetAll();
            var personsInLocation = allPersons
                .Where(p => p.Contacts.Any(c => 
                    c.Type == ContactType.Location && 
                    c.Content.Equals(report.Location, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            report.PersonCount = personsInLocation.Count;
            report.PhoneCount = personsInLocation
                .SelectMany(p => p.Contacts)
                .Count(c => c.Type == ContactType.Phone);
            report.Status = ReportStatus.Completed;

            return await Task.FromResult(_reportRepository.Update(report));
        }

        // Implement other interface methods...
    }
} 