using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace TaskManager.Api.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {}

        public DbSet<TaskItem> Tasks {get; set;}
    }
}