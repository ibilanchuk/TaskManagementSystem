using MediatR;

namespace TaskManagementSystem.Application.UpdateTask;

public class UpdateTaskHandler : IRequestHandler<UpdateTask, Unit>
{
    public Task<Unit> Handle(UpdateTask request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Updated task with ID: {request.TaskId}");
        return Task.FromResult(Unit.Value);
    }
}