using Complex_Task_Manager_Endrit_Ajrulla.Server.Models;

namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Validation
{
    public class TaskItemValidator : IValidator<TaskItem>
    {
        public ValidationResult Validate(TaskItem taskItem)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(taskItem.Name))
            {
                result.Errors.Add("Task name is required.");
            }
            else if (taskItem.Name.Length > 100)
            {
                result.Errors.Add("Task name must be less than 100 characters.");
            }

            if (taskItem.DueDate < DateTime.Now)
            {
                result.Errors.Add("Due date must be in the future.");
            }

            if (string.IsNullOrWhiteSpace(taskItem.Description))
            {
                result.Errors.Add("Task description is required.");
            }
            else if (taskItem.Description.Length > 500)
            {
                result.Errors.Add("Task description must be less than 500 characters.");
            }

            if (string.IsNullOrWhiteSpace(taskItem.Category))
            {
                result.Errors.Add("Task category is required.");
            }

            if (string.IsNullOrWhiteSpace(taskItem.Status))
            {
                result.Errors.Add("Task status is required.");
            }
            else if (!new[] { "Incomplete", "Complete" }.Contains(taskItem.Status))
            {
                result.Errors.Add("Task status must be either 'Incomplete' or 'Complete'.");
            }

            result.IsValid = !result.Errors.Any();
            return result;
        }
    }

}
