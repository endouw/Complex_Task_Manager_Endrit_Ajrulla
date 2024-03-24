namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Models
{
    public class TaskHistory
    {
        public int Id { get; set; }
        public int TaskItemId { get; set; }
        public string UserId { get; set; }
        public string ChangedField { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Timestamp { get; set; }

        public TaskItem TaskItem { get; set; }
        public ApplicationUser User { get; set; }
    }
}
