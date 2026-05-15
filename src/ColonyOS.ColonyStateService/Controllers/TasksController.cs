using ColonyOS.Contracts.Models.Requests;
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
        private readonly IColonyStateService _colonyStateService;

        public TasksController(ITaskService taskService, IColonyStateService colonyStateService)
        {
            _taskService = taskService;
            _colonyStateService = colonyStateService;
        }

        [HttpGet]
        public ActionResult<IReadOnlyList<TaskItem>> GetActiveTasks(CancellationToken cancellationToken)
        {
            var tasks = _taskService.GetActiveTasks(cancellationToken);
            return Ok(tasks);
        }

        [HttpPost]
        public ActionResult<TaskItem> CreateTask([FromBody] CreateTaskRequest createTaskRequest, CancellationToken cancellationToken)
        {
            var createdTask = _taskService.CreateTask(createTaskRequest);
            return CreatedAtAction(nameof(GetActiveTasks), new { id = createdTask.Id }, createdTask);
        }

        [HttpPost("{taskId:guid}/assign-crew")]
        public async Task<ActionResult<TaskItem>> AssignCrewToTask(Guid taskId, [FromBody] AssignCrewToTaskRequest request, CancellationToken cancellationToken)
        {
            request.TaskId = taskId;

            var task = await _colonyStateService.AssignCrewToTaskAsync(request, cancellationToken);

            if (task == null)
                return BadRequest("Unable to assign crew member to task.");

            return Ok(task);
        }

        [HttpPatch("status")]
        public ActionResult<TaskItem> UpdateTaskStatus([FromBody] UpdateTaskStatusRequest updateTaskStatusRequest, CancellationToken cancellationToken)
        {
            var updatedTask = _taskService.UpdateTaskStatus(updateTaskStatusRequest);

            if (updatedTask is null)
                return NotFound();

            return Ok(updatedTask);
        }
    }
}
