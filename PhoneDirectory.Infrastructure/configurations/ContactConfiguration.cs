using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneDirectory.Infrastructure.Entities;

public class ContactConfiguration : IEntityTypeConfiguration<ContactEntity>
{
    public void Configure(EntityTypeBuilder<ContactEntity> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.ContactType)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(200);
    }
} 