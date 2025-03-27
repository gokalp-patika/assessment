using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhoneDirectory.Domain.Interfaces;
using PhoneDirectory.Infrastructure.Repositories;

namespace PhoneDirectory.Infrastructure.Configurations
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<PhoneDirectoryDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(PhoneDirectoryDbContext).Assembly.FullName)
                );
            });

            // Register Repositories
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();

            return services;
        }
    }
} 