using System.ComponentModel.DataAnnotations;
namespace TaskManager.Api.Dtos;

public class UpdateTaskDto
{
    [Required, StringLength(100)]
    public string Title {get; set;} = "";

    public DateTime? DueDate {get; set;}

    public bool IsCompleted {get; set;}
}