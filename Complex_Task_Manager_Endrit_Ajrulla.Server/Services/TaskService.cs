using Complex_Task_Manager_Endrit_Ajrulla.Server.Models;
using Complex_Task_Manager_Endrit_Ajrulla.Server.Repository;
using Complex_Task_Manager_Endrit_Ajrulla.Server.Validation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IValidator<TaskItem> _taskValidator;
        private readonly UserManager<ApplicationUser> _userManager;


        public TaskService(ITaskRepository taskRepository, IValidator<TaskItem> taskValidator, UserManager<ApplicationUser> usermanager)
        {
            _taskRepository = taskRepository;
            _taskValidator = taskValidator;
            _userManager = usermanager;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllTasksAsync();
        }


        public async Task<IEnumerable<DueDateModelReminder>> GetAllTasksAsyncDueIn24()
        {

            return await _taskRepository.GetAllTasksAsyncDueIn24();
        }




        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItemDto taskItemDto)
        {
           var user =  await _userManager.FindByIdAsync(taskItemDto.UserId);
            var taskItem = new TaskItem()
            {
                Id = taskItemDto.Id,
                Name = taskItemDto.Name,
                DueDate = taskItemDto.DueDate,
                Description = taskItemDto.Description,
                Category = taskItemDto.Category,
                Status = taskItemDto.Status,
                UserId = user.Id,
                User = user,
            };
            ValidateTaskItem(taskItem);
            return await _taskRepository.CreateTaskAsync(taskItem);
        }

        public async Task UpdateTaskAsync(TaskItemDto taskItemDto)
        {
            var user = await _userManager.FindByIdAsync(taskItemDto.UserId);
            var taskItem = new TaskItem()
            {
                Id = taskItemDto.Id,
                Name = taskItemDto.Name,
                DueDate = taskItemDto.DueDate,
                Description = taskItemDto.Description,
                Category = taskItemDto.Category,
                Status = taskItemDto.Status,
                UserId = user.Id,
                User = user,
            };
            ValidateTaskItem(taskItem);
            await _taskRepository.UpdateTaskAsync(taskItem);
        }

        public async Task DeleteTaskAsync(int id)
        {
            var taskItem = await GetTaskByIdAsync(id);
            if (taskItem == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} not found");
            }

            await _taskRepository.DeleteTaskAsync(id);
        }

        private void ValidateTaskItem(TaskItem taskItem)
        {
            var validationResult = _taskValidator.Validate(taskItem);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors);
                throw new ValidationException(errorMessage);
            }
        }

        public async Task<List<TaskHistory>> GetTaskHistoryById(int taskId)
        {
            var taskHistory = await  _taskRepository.GetTaskHistoryByIdAsync(taskId);

            return taskHistory;
        }
    }

}
