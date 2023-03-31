namespace TaskManagementSystem.Domain;

public interface ITaskRepository
{
    Task<IReadOnlyCollection<Task>> GetAllAsync(CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddAsync(Task task, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task UpdateStatusAsync(int taskId, TaskStatus status, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task<Task?> FindByIdAsync(int taskId, CancellationToken cancellationToken = default);
}