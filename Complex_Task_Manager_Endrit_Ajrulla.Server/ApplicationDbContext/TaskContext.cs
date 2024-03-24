using Complex_Task_Manager_Endrit_Ajrulla.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Complex_Task_Manager_Endrit_Ajrulla.Server.ApplicationDbContext
{
    public class TaskContext : IdentityDbContext<ApplicationUser>
    {
        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between TaskItem and ApplicationUser
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.User)
                .WithMany() 
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // Configure the relationship between TaskHistory and TaskItem
            modelBuilder.Entity<TaskHistory>()
                .HasOne(th => th.TaskItem)
                .WithMany(ti => ti.TaskHistories)
                .HasForeignKey(th => th.TaskItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
