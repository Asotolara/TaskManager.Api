using TaskManager.Api.Models;
using TaskManager.Api.Dtos;
using TaskManager.Api.Repositories;


namespace TaskManager.Api.Services;

public class TaskService: ITaskService
{

    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    private static TaskDto ToDto(TaskItem t) => new TaskDto
    {
        Id = t.Id,
        Title = t.Title,
        DueDate = t.DueDate,
        IsCompleted = t.IsCompleted
    };

    public async Task<List<TaskDto>> GetAllAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return tasks.Select(ToDto).ToList();
    }

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if(task is null)
        {
            return null;
        }

        return ToDto(task);
        
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto taskDto)
    {
        var task = new TaskItem
        {
            Title = taskDto.Title,
            DueDate = taskDto.DueDate,
            IsCompleted = false

        };

        await _taskRepository.AddAsync(task);
        return ToDto(task);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTaskDto updatedTask)
    {
        var task  = await _taskRepository.GetByIdAsync(id);

        if(task is null)
        {
            return false;
        }

        task.Title = updatedTask.Title;
        task.DueDate = updatedTask.DueDate;
        task.IsCompleted = updatedTask.IsCompleted;
        
        await _taskRepository.UpdateAsync(task);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if(task is null)
        {
            return false;
        }

        await _taskRepository.DeleteAsync(task);
        return true;
    }
}