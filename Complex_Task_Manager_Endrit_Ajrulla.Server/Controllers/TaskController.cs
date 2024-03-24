using Complex_Task_Manager_Endrit_Ajrulla.Server.Models;
using Complex_Task_Manager_Endrit_Ajrulla.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Complex_Task_Manager_Endrit_Ajrulla.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly UserManager<ApplicationUser> _userManager;


        public TaskController(ITaskService taskService, UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _userManager = userManager; 
        }


        [HttpGet]
        [Route("getAllTasks")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userEmail);
            var tasks = await _taskService.GetAllTasksAsync();
            var sortedTasks = tasks.Where(t => t.UserId == user.Id).ToList();
            return Ok(sortedTasks);
        }

        [HttpGet("{id}")]
        [Authorize]

        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        [Authorize]

        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItemDto taskItem)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var createdTask = await _taskService.CreateTaskAsync(taskItem);
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);

        }

        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItemDto taskItem)
        {
            if (id != taskItem.Id) return BadRequest("Task ID mismatch");

            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null) return NotFound();

            await _taskService.UpdateTaskAsync(taskItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }

        [HttpGet("tasksDueIn24Hours")]
        public async Task<ActionResult<IEnumerable<DueDateModelReminder>>> GetTasksDueIn24Hours()
        {
            var tasks = await _taskService.GetAllTasksAsyncDueIn24();

            return Ok(tasks);
        }

        [HttpGet("history/{taskId}")]
        [Authorize]

        public async Task<ActionResult> GetTaskHistory(int taskId)
        {
            var history = await _taskService.GetTaskHistoryById(taskId);
            return Ok(history);
        }
    }

}
