using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain;
using Task = System.Threading.Tasks.Task;
using TaskStatus = TaskManagementSystem.Domain.TaskStatus;

namespace TaskManagementSystem.Infrastructure.Persistence;

public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _dbContext;

    public TaskRepository(TaskDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Domain.Task>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return (await _dbContext.Tasks.ToListAsync(cancellationToken: cancellationToken))
            .AsReadOnly();
    }

    public async Task AddAsync(Domain.Task task, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(task, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateStatusAsync(int taskId, Domain.TaskStatus status, CancellationToken cancellationToken = default)
    {
        var task = await _dbContext.Tasks.FirstAsync(x => x.Id == taskId, cancellationToken: cancellationToken);
        
        task.SetStatus(status);
        
        _dbContext.Update(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Domain.Task?> FindByIdAsync(int taskId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken: cancellationToken);
    }
}