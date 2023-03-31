namespace TaskManagementSystem.Domain;

public class Task
{
    public Task(
        string name,
        string description,
        TaskStatus status,
        string? assignedTo = null)
    {
        Name = name;
        Description = description;
        Status = status;
        AssignedTo = assignedTo;
    }

    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public TaskStatus Status { get; private set; }
    public string? AssignedTo { get; init; }
    
    public void SetStatus(TaskStatus status)
    {
        Status = status;
    }
}