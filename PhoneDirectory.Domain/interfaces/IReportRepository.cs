using System;
using System.Collections.Generic;
using PhoneDirectory.Domain.Entities;

namespace PhoneDirectory.Domain.Interfaces
{
    public interface IReportRepository
    {
        Report Add(Report report);
        Report Update(Report report);
        Report GetById(Guid id);
        IEnumerable<Report> GetAll();
    }
}