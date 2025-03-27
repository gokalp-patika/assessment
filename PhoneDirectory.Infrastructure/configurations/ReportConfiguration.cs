using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneDirectory.Infrastructure.Entities;

public class ReportConfiguration : IEntityTypeConfiguration<ReportEntity>
{
    public void Configure(EntityTypeBuilder<ReportEntity> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");
            
        builder.Property(r => r.Status)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(r => r.Location)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(r => r.RequestedDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
        builder.Property(r => r.PersonCount)
            .IsRequired();
            
        builder.Property(r => r.PhoneCount)
            .IsRequired();
    }
} 