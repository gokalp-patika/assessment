using System;
using System.Collections.Generic;
using System.Linq;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Domain.Interfaces;
using PhoneDirectory.Infrastructure.Entities;
using PhoneDirectory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace PhoneDirectory.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly PhoneDirectoryDbContext _dbContext;

        public ReportRepository(PhoneDirectoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Report> GetByIdAsync(Guid id)
        {
            var entity = await _dbContext.Reports.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Report with ID {id} not found");
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map report");
        }

        public Report Add(Report report)
        {
            if (report == null) throw new ArgumentNullException(nameof(report));
            
            var entity = report.ToEntity() ?? throw new InvalidOperationException("Failed to map report to entity");
            _dbContext.Reports.Add(entity);
            _dbContext.SaveChanges();
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map created report");
        }

        public Report Update(Report report)
        {
            if (report == null) throw new ArgumentNullException(nameof(report));
            
            var entity = report.ToEntity() ?? throw new InvalidOperationException("Failed to map report to entity");
            _dbContext.Reports.Update(entity);
            _dbContext.SaveChanges();
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map updated report");
        }

        public Report GetById(Guid id)
        {
            var entity = _dbContext.Reports.FirstOrDefault(r => r.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Report with ID {id} not found");
            return entity.ToDomain() ?? throw new InvalidOperationException("Failed to map report");
        }

        public IEnumerable<Report> GetAll()
        {
            return _dbContext.Reports
                .AsNoTracking()
                .ToList()
                .Select(e => e.ToDomain() ?? throw new InvalidOperationException("Failed to map report"));
        }
    }
}