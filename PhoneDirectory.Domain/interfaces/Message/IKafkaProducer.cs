using System.Threading.Tasks;
using PhoneDirectory.Domain.Models;

namespace PhoneDirectory.Domain.Interfaces.Message
{
    public interface IKafkaProducer
    {
        Task PublishReportRequestAsync(ReportRequest request);
    }
} 