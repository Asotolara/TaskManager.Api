using TaskManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Data;
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

    [HttpGet]
    public async Task<ActionResult<List<TaskItem>>> GetAllTasks()
    {
        var tasks = await _context.Tasks.ToListAsync();

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
    public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TaskItem updatedTask)
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