using MediatR;
using TaskManagementSystem.Domain;

namespace TaskManagementSystem.Application.UpdateTask;

public class UpdateTaskHandler : IRequestHandler<UpdateTask, Unit>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Unit> Handle(UpdateTask request, CancellationToken cancellationToken)
    {
        var existingTask = await _taskRepository.FindByIdAsync(request.TaskId, cancellationToken);
        if (existingTask == null)
        {
            throw new ArgumentException($"Could not find task with id: {request.TaskId}");
        }

        await _taskRepository.UpdateStatusAsync(request.TaskId, request.NewStatus, cancellationToken);
        
        Console.WriteLine($"Updated task with ID: {request.TaskId}");
        return Unit.Value;
    }
}