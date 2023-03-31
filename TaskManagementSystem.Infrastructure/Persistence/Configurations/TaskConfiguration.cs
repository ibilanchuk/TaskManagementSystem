using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagementSystem.Infrastructure.Persistence.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Domain.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Task> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(t => t.Description)
            .HasMaxLength(200)
            .IsRequired();
    }
}