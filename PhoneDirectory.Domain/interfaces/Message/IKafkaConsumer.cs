using System.Threading.Tasks;

namespace PhoneDirectory.Domain.Interfaces.Message
{
    public interface IKafkaConsumer
    {
        Task StartConsumingReportRequests();
        Task StopConsumingAsync();
    }
} 