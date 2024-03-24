using Complex_Task_Manager_Endrit_Ajrulla.Server.Models;

namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Repository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<IEnumerable<DueDateModelReminder>> GetAllTasksAsyncDueIn24();
        Task<TaskItem> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(TaskItem taskItem);
        Task UpdateTaskAsync(TaskItem taskItem);
        Task DeleteTaskAsync(int id);
        Task LogTaskChangeAsync(TaskItem task, string fieldName, string oldValue, string newValue, string userId);
        Task<List<TaskHistory>> GetTaskHistoryByIdAsync(int taskId, int page = 1, int pageSize = 5);


    }

}
