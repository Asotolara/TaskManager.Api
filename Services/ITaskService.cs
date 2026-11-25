using TaskManager.Api.Dtos;
namespace TaskManager.Api.Services;
public interface ITaskService
{
    Task<List<TaskDto>> GetAllAsync(string userId);
    Task<TaskDto?> GetByIdAsync(int id, string userId);          
    Task<TaskDto> CreateAsync(CreateTaskDto dto, string userId);
    Task<bool> UpdateAsync(int id, UpdateTaskDto dto, string userId);   
    Task<bool> DeleteAsync(int id, string userId);                      
}