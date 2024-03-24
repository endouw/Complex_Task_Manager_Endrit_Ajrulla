using Complex_Task_Manager_Endrit_Ajrulla.Server.ApplicationDbContext;

namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Services
{
    public class NotificationService
    {
        private readonly ITaskService _service;
        private readonly EmailService _emailService;

        public NotificationService(ITaskService service, EmailService emailService)
        {
            _service = service;
            _emailService = emailService;
        }

        public async Task SendTaskReminders()
        {
            var upcomingTasks = await _service.GetAllTasksAsyncDueIn24();

            foreach (var task in upcomingTasks)
            {
                var subject = "Task Reminder";
                var body = $"Reminder: Your task '{task.Name}' is due on {task.DueDate}.";
                await _emailService.SendEmailAsync(task.UserEmail, subject, body);
            }
        }
    }

}
