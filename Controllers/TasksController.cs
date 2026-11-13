using TaskManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Dtos;
using TaskManager.Api.Repositories;
using System.Reflection.Metadata.Ecma335;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{

    private readonly ITaskRepository _taskRepository;

    public TasksController(ITaskRepository taskRepository)
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


    [HttpGet]
    public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return tasks.Select(ToDto).ToList();

        
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTaskById(int id)
    {

        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        var result = ToDto(task);
        
        return result;
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto taskDto)
    {

        var task = new TaskItem
        {
            Title = taskDto.Title,
            DueDate = taskDto.DueDate,
            IsCompleted = false

        };

        await _taskRepository.AddAsync(task);
        

        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, ToDto(task));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto updatedTask)
    {
        var task  = await _taskRepository.GetByIdAsync(id);

        if(task is null)
        {
            return NotFound();
        }

        task.Title = updatedTask.Title;
        task.DueDate = updatedTask.DueDate;
        task.IsCompleted = updatedTask.IsCompleted;
        
        await _taskRepository.UpdateAsync(task);
        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if(task is null)
        {
            return NotFound();
        }

        await _taskRepository.DeleteAsync(task);
        return NoContent();
    }
}