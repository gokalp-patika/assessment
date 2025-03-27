using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using PhoneDirectory.Domain.Models;
using PhoneDirectory.Domain.Interfaces.Message;
using Microsoft.Extensions.Logging;

namespace PhoneDirectory.Infrastructure.Messaging
{
    public class KafkaProducer : IKafkaProducer, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;
        private const string TOPIC = "report-requests";

        public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer> logger)
        {
            _logger = logger;
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                EnableDeliveryReports = true,
                Acks = Acks.All
            };

            _producer = new ProducerBuilder<string, string>(config)
                .SetErrorHandler((_, e) => _logger.LogError($"Kafka error: {e.Reason}"))
                .Build();

            ValidateConnection();
        }

        private void ValidateConnection()
        {
            try
            {
                // Simple connection test by trying to produce a test message
                var testMessage = new Message<string, string>
                {
                    Key = "test",
                    Value = "connection-test"
                };

                var result = _producer.ProduceAsync(TOPIC, testMessage).GetAwaiter().GetResult();
                _logger.LogInformation($"Successfully connected to Kafka. Test message delivered to {result.Topic} [{result.Partition}]");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to Kafka");
                throw new Exception("Failed to establish Kafka connection", ex);
            }
        }

        public async Task PublishReportRequestAsync(ReportRequest request)
        {
            try
            {
                var message = JsonSerializer.Serialize(request);
                var result = await _producer.ProduceAsync(TOPIC, new Message<string, string>
                {
                    Key = request.ReportId.ToString(),
                    Value = message
                });

                _logger.LogInformation("Message delivered to {Topic} [{Partition}] at offset {Offset}",
                    result.Topic, result.Partition, result.Offset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish message for report {ReportId}", request.ReportId);
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                _producer?.Flush(TimeSpan.FromSeconds(5));
                _producer?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during producer disposal");
            }
        }
    }
} 