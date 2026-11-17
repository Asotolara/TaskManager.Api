using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace TaskManager.Api.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter a title")]
    [StringLength(100, ErrorMessage = "title must be 100 characters or shorter ")]
    public required string Title { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    public string? OwnerId { get; set; }

    public IdentityUser? Owner { get; set; }

}