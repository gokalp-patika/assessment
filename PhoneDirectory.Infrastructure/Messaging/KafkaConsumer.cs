using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using PhoneDirectory.Domain.Models;
using PhoneDirectory.Domain.Interfaces.Services;
using PhoneDirectory.Domain.Interfaces.Message;
using PhoneDirectory.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace PhoneDirectory.Infrastructure.Messaging
{
    public class KafkaConsumer : IKafkaConsumer, IDisposable
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<KafkaConsumer> _logger;
        private const string TOPIC = "report-requests";
        private bool _consuming = false;

        public KafkaConsumer(
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<KafkaConsumer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"] ?? "report-processing-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                EnablePartitionEof = true
            };

            _consumer = new ConsumerBuilder<string, string>(config)
                .SetErrorHandler((_, e) => _logger.LogError($"Kafka error: {e.Reason}"))
                .Build();

            ValidateConnection();
        }

        private void ValidateConnection()
        {
            try
            {
                _consumer.Subscribe(TOPIC);
                _consumer.Unsubscribe();
                _logger.LogInformation($"Successfully validated Kafka consumer connection for topic '{TOPIC}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to Kafka");
                throw new Exception("Failed to establish Kafka connection", ex);
            }
        }

        public async Task StartConsumingReportRequests()
        {
            _consuming = true;
            _consumer.Subscribe(TOPIC);
            _logger.LogInformation("Started consuming messages from topic: {Topic}", TOPIC);

            while (_consuming)
            {
                try
                {
                    var consumeResult = _consumer.Consume();
                    if (consumeResult?.Message?.Value == null) continue;

                    var request = JsonSerializer.Deserialize<ReportRequest>(consumeResult.Message.Value);
                    if (request == null) continue;

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();
                        var report = await reportService.GetReportByIdAsync(request.ReportId);
                        if (report != null)
                        {
                            await reportService.ProcessReportAsync(report);
                            _consumer.Commit(consumeResult);
                            _logger.LogInformation("Successfully processed report: {ReportId}", report.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                }
            }
        }

        public Task StopConsumingAsync()
        {
            _consuming = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _consumer?.Close();
            _consumer?.Dispose();
        }
    }
} 