using TaskManagementSystem.Application.Abstractions;

namespace TaskManagementSystem.Application.UpdateTask;

public class UpdateTask : AsyncCommand
{
    public UpdateTask(
        int taskId,
        Domain.TaskStatus newStatus,
        string updatedBy)
    {
        TaskId = taskId;
        NewStatus = newStatus;
        UpdatedBy = updatedBy;
    }
    
    public int TaskId { get; init; }
    public Domain.TaskStatus NewStatus { get; init; }
    public string UpdatedBy { get; init; }
}