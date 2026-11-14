using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Dtos;
using TaskManager.Api.Services;


[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{

    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
    {
         return await _taskService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTaskById(int id)
    {

        var task = await _taskService.GetByIdAsync(id);
        if (task is null)
        {
            return NotFound();
        }
        
        return task;
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto taskDto)
    {

        var createdTask = await _taskService.CreateAsync(taskDto);
        return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto updatedTask)
    {
        var taskFound  = await _taskService.UpdateAsync(id, updatedTask);
        if (!taskFound)
        {
            return NotFound();
        } 


        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var taskDeleted = await _taskService.DeleteAsync(id);

        if(!taskDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}