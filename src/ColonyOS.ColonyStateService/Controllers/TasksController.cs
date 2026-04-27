using ColonyOS.ColonyStateService.Models.Requests;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Models.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ColonyOS.ColonyStateService.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TaskItem>>> GetActiveTasks(CancellationToken cancellationToken)
        {
            var tasks = _taskService.GetActiveTasks(cancellationToken);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] CreateTaskRequest createTaskRequest, CancellationToken cancellationToken)
        {
            var createdTask = await _taskService.CreateTaskAsync(createTaskRequest, cancellationToken);
            return CreatedAtAction(nameof(GetActiveTasks), new { id = createdTask.Id }, createdTask);
        }

        [HttpPatch("{taskId:guid}/status")]
        public async Task<ActionResult<TaskItem>> UpdateTaskStatus(Guid taskId,
            [FromBody] UpdateTaskStatusRequest updateTaskStatusRequest,
            CancellationToken cancellationToken)
        {
            var updatedTask = await _taskService.UpdateTaskStatusAsync(taskId, updateTaskStatusRequest.Status, cancellationToken);

            if (updatedTask is null)
                return NotFound();

            return Ok(updatedTask);
        }
    }
}
