using Complex_Task_Manager_Endrit_Ajrulla.Server.ApplicationDbContext;
using Complex_Task_Manager_Endrit_Ajrulla.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;

        public TaskRepository(TaskContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<IEnumerable<DueDateModelReminder>> GetAllTasksAsyncDueIn24()
        {
            DateTime now = DateTime.Now;
            DateTime twentyFourHoursFromNow = now.AddHours(24);

            var tasks =  await _context.Tasks.ToListAsync();

            var tasksDueIn24Hours = tasks.Where(task => task.DueDate > now && task.DueDate <= twentyFourHoursFromNow).ToList();

            var listOfUserIds = tasksDueIn24Hours.Select(a => a.UserId).ToList();

            var emails = await _context.Users.Where(a => listOfUserIds.Contains(a.Id)).Select(a => a.Email).ToListAsync();

            var tasksDueModelList = new List<DueDateModelReminder>() { };

            tasksDueModelList = tasksDueIn24Hours
            .SelectMany(task => emails, (task, email) => new DueDateModelReminder
            {
                Name = task.Name,
                DueDate = task.DueDate,
                Description = task.Description,
                Category = task.Category,
                Status = task.Status,
                UserEmail = email
            })
            .ToList();

            return tasksDueModelList;
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem)
        {
            _context.Tasks.Add(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task UpdateTaskAsync(TaskItem updatedTaskItem)
        {
     
            var existingTask = await _context.Tasks.FindAsync(updatedTaskItem.Id);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with ID {updatedTaskItem.Id} not found");
            }

            // Check and update each property, logging changes if necessary
            if (existingTask.Name != updatedTaskItem.Name)
            {
                await LogTaskChangeAsync(existingTask, "Name", existingTask.Name, updatedTaskItem.Name, updatedTaskItem.UserId);
                existingTask.Name = updatedTaskItem.Name;
            }

            if (existingTask.DueDate != updatedTaskItem.DueDate)
            {
                await LogTaskChangeAsync(existingTask, "DueDate", existingTask.DueDate.ToString(), updatedTaskItem.DueDate.ToString(), updatedTaskItem.UserId);
                existingTask.DueDate = updatedTaskItem.DueDate;
            }

            if (existingTask.Category != updatedTaskItem.Category)
            {
                await LogTaskChangeAsync(existingTask, "Category", existingTask.Category, updatedTaskItem.Category, updatedTaskItem.UserId);
                existingTask.Category = updatedTaskItem.Category;
            }

            if (existingTask.Status != updatedTaskItem.Status)
            {
                await LogTaskChangeAsync(existingTask, "Status", existingTask.Status, updatedTaskItem.Status, updatedTaskItem.UserId);
                existingTask.Status = updatedTaskItem.Status;
            }

            if (existingTask.Description != updatedTaskItem.Description)
            {
                await LogTaskChangeAsync(existingTask, "Description", existingTask.Description, updatedTaskItem.Description, updatedTaskItem.UserId);
                existingTask.Description = updatedTaskItem.Description;
            }

            // Save the changes to the database
            _context.Tasks.Update(existingTask);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem != null)
            {
                _context.Tasks.Remove(taskItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task LogTaskChangeAsync(TaskItem task, string fieldName, string oldValue, string newValue, string userId)
        {
            var taskHistory = new TaskHistory
            {
                TaskItemId = task.Id,
                UserId = userId,
                ChangedField = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                Timestamp = DateTime.UtcNow
            };

            _context.TaskHistories.Add(taskHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TaskHistory>> GetTaskHistoryByIdAsync(int taskId, int page = 1, int pageSize = 5)
        {
            var history = await _context.TaskHistories
                .Where(h => h.TaskItemId == taskId)
                .Include(h => h.User)
                .OrderByDescending(h => h.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return history;
        }
    }

}
