using TaskManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Data;
using TaskManager.Api.Dtos;
using Microsoft.Extensions.Validation;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{

    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
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

        var tasks = await _context.Tasks.Select(t => ToDto(t)).ToListAsync();

        return tasks;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTaskById(int id)
    {

        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        return task;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask(CreateTaskDto taskDto)
    {

        var task = new TaskItem
        {
            Title = taskDto.Title,
            DueDate = taskDto.DueDate,
            IsCompleted = false

        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto updatedTask)
    {
        var task  = await _context.Tasks.FindAsync(id);

        if(task is null)
        {
            return NotFound();
        }

        task.Title = updatedTask.Title;
        task.DueDate = updatedTask.DueDate;
        task.IsCompleted = updatedTask.IsCompleted;
        
        await _context.SaveChangesAsync();
        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if(task is null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await  _context.SaveChangesAsync();
        return NoContent();
    }
}