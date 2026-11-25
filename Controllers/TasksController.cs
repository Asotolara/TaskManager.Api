using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Dtos;
using TaskManager.Api.Services;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{

    private readonly ITaskService _taskService;

    private string? GetUserId() => User.FindFirstValue("sub");

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
    {
        var userId = GetUserId();
        if(userId is null)
        {
            return Unauthorized();
        }
        return await _taskService.GetAllAsync(userId);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTaskById(int id)
    {
        var userId = GetUserId();
        if(userId is null)
        {
            return Unauthorized();
        }

        var task = await _taskService.GetByIdAsync(id, userId);
        if (task is null)
        {
            return NotFound();
        }
        
        return task;
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto taskDto)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }
        var createdTask = await _taskService.CreateAsync(taskDto, userId);
        return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto updatedTask)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }
        var taskFound  = await _taskService.UpdateAsync(id, updatedTask, userId);
        if (!taskFound)
        {
            return NotFound();
        } 


        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }
        var taskDeleted = await _taskService.DeleteAsync(id, userId);

        if(!taskDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}