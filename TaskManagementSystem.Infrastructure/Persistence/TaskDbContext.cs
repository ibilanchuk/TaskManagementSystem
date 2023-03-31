using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Infrastructure.Persistence;

public class TaskDbContext : DbContext
{
    public DbSet<Domain.Task> Tasks { get; set; }

    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}