namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

}
