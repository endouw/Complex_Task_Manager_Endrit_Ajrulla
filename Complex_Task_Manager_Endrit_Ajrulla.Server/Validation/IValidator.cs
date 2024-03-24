using Complex_Task_Manager_Endrit_Ajrulla.Server.Models;

namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Validation
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T entity);
    }

}
