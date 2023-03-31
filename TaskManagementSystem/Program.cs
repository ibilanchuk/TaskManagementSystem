// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem;
using TaskManagementSystem.Application.UpdateTask;
using TaskManagementSystem.Domain;
using TaskManagementSystem.Infrastructure.Messaging;
using Task = TaskManagementSystem.Domain.Task;
using TaskStatus = TaskManagementSystem.Domain.TaskStatus;

var services = new ServiceCollection();
var startup = new Startup();
startup.ConfigureServices(services);

var serviceProvider = services.BuildServiceProvider();
var serviceBus = serviceProvider.GetRequiredService<IServiceBus>();

var option = 0;

while (option != 4)
{
    Console.Clear();
    Console.WriteLine("Task Manager");
    Console.WriteLine("1. Create task");
    Console.WriteLine("2. Update task");
    Console.WriteLine("3. Display tasks");
    Console.WriteLine("4. Exit");
    Console.Write("Enter your choice: ");
    option = int.Parse(Console.ReadLine() ?? string.Empty);

    using var scope = serviceProvider.CreateScope();
    var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
    
    switch (option)
    {
        case 1:
            await CreateTaskAsync(taskRepository);
            break;
        case 2:
            await UpdateTaskAsync();
            Console.WriteLine("Task updated successfully.");
            break;
        case 3:
            foreach (var task in await GetTasksAsync(taskRepository))
            {
                Console.WriteLine($"Existing tasks: {JsonSerializer.Serialize(task)}");
            }

            break;
        case 4:
            Console.WriteLine("Exiting..");
            return;
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

async System.Threading.Tasks.Task CreateTaskAsync(ITaskRepository taskRepository)
{
    Console.Clear();
    Console.WriteLine("Create a new task");
    Console.WriteLine("-----------------");

    Console.Write("Enter task name: ");
    var name = Console.ReadLine() ?? string.Empty;

    Console.Write("Enter task description: ");
    var description = Console.ReadLine() ?? string.Empty;

    var task = new Task(name, description, TaskStatus.NotStarted);
    await taskRepository.AddAsync(task);
  
    Console.WriteLine($"Task {JsonSerializer.Serialize(task)} created successfully.");
}


async System.Threading.Tasks.Task UpdateTaskAsync()
{
    Console.Clear();
    Console.WriteLine("Update an existing task");
    Console.WriteLine("-----------------------");

    // Enter task ID
    Console.Write("Enter task ID: ");
    int.TryParse(Console.ReadLine(), out int taskId);

    // Choose new status
    Console.WriteLine("Enter new task status: 0 - NotStarted, 1 - InProgress, 2 - Completed");
    Enum.TryParse(Console.ReadLine(), out TaskStatus status);


    // Enter task description
    Console.Write("Enter who updated the status: ");
    var updatedBy = Console.ReadLine();
    
    await serviceBus.SendMessageAsync(new UpdateTask(taskId, status, updatedBy));
    Console.WriteLine($"Command to update task with id: {taskId} was sent successfully");
}

async Task<IEnumerable<Task>> GetTasksAsync(ITaskRepository taskRepository)
{
    return await taskRepository.GetAllAsync();
}