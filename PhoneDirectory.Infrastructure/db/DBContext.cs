using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Infrastructure.Configurations;
using PhoneDirectory.Infrastructure.Entities;

namespace PhoneDirectory.Infrastructure
{
    public class PhoneDirectoryDbContext : DbContext
    {
        public PhoneDirectoryDbContext(DbContextOptions<PhoneDirectoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<ReportEntity> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfiguration());
        }
    }
}