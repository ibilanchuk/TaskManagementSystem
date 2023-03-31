using FluentAssertions;
using Moq;
using TaskManagementSystem.Application.UpdateTask;
using TaskManagementSystem.Domain;
using Task = TaskManagementSystem.Domain.Task;
using TaskStatus = TaskManagementSystem.Domain.TaskStatus;

namespace TaskManagementSystem.Application.UnitTests.Handlers;

public class UpdateTaskHandlerTests
{
    private readonly UpdateTaskHandler _sut;
    private readonly Mock<ITaskRepository> _taskRepository = new Mock<ITaskRepository>();
    
    public UpdateTaskHandlerTests()
    {
        _sut = new UpdateTaskHandler(_taskRepository.Object);
    }

    [Test]
    public async System.Threading.Tasks.Task Update_WhenDoesTaskNotExist_ShouldThrow()
    {
        var message = new UpdateTask.UpdateTask(1, TaskStatus.InProgress, "anyone");
        
        var act = async () => await _sut.Handle(message, CancellationToken.None);
        await act.Should().ThrowAsync<ArgumentException>();
    }
    
    [Test]
    public async System.Threading.Tasks.Task Update_WhenItemExists()
    {
        var message = new UpdateTask.UpdateTask(1, TaskStatus.Completed, "anyone");
        var task = new Task("some task", "desc", TaskStatus.InProgress);
        
        _taskRepository.Setup(x => x.FindByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);
        
        await _sut.Handle(message, CancellationToken.None);

        _taskRepository.Verify(
            x => x.UpdateStatusAsync(message.TaskId, message.NewStatus, It.IsAny<CancellationToken>()),
            Times.Exactly(1));
    }
}