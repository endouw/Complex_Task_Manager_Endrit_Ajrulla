using Complex_Task_Manager_Endrit_Ajrulla.Server.Models;

namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<IEnumerable<DueDateModelReminder>> GetAllTasksAsyncDueIn24();
        Task<TaskItem> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(TaskItemDto taskItem);
        Task UpdateTaskAsync(TaskItemDto taskItem);
        Task DeleteTaskAsync(int id);
        Task<List<TaskHistory>> GetTaskHistoryById(int taskId);

    }
}
