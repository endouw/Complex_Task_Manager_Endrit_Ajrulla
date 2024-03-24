namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<TaskHistory> TaskHistories { get; set; }

    }

}
