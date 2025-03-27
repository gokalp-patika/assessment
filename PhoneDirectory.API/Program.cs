using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhoneDirectory.Application.Services;
using PhoneDirectory.Domain.Interfaces.Message;
using PhoneDirectory.Domain.Interfaces.Services;
using PhoneDirectory.Infrastructure.Configurations;
using PhoneDirectory.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container
builder.Services.AddControllers();

// Add Infrastructure Services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Application Services
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Add Kafka Services
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>();

// Add other services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Phone Directory API", 
        Version = "v1",
        Description = "A simple phone directory API with report generation capabilities"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Phone Directory API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at root
    });
}

// Add CORS middleware (must be before UseHttpsRedirection and UseAuthorization)
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Start Kafka consumer with proper lifetime management
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var kafkaConsumer = app.Services.GetRequiredService<IKafkaConsumer>();
var cancellationTokenSource = new CancellationTokenSource();

var consumerTask = Task.Run(async () => 
{
    try
    {
        await kafkaConsumer.StartConsumingReportRequests();
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error in Kafka consumer");
    }
}, cancellationTokenSource.Token);

lifetime.ApplicationStopping.Register(() => 
{
    cancellationTokenSource.Cancel();
    kafkaConsumer.StopConsumingAsync().Wait();
});

app.Run();
