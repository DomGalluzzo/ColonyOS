using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Models.Requests;
using ColonyOS.Contracts.Models.Alerts;
using ColonyOS.Contracts.Models.Tasks;
using ColonyOS.Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColonyOS.Gateway.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IColonyStateGatewayClient _colonyStateGatewayClient;

        public DashboardController(IColonyStateGatewayClient colonyStateGatewayClient)
        {
            _colonyStateGatewayClient = colonyStateGatewayClient;
        }

        [HttpGet("state")]
        [ProducesResponseType(typeof(ColonyState), StatusCodes.Status200OK)]
        public async Task<ActionResult<ColonyState>> GetStateAsync(CancellationToken cancellationToken)
        {
            var state = await _colonyStateGatewayClient.GetCurrentStateAsync(cancellationToken);
            return Ok(state);
        }

        [HttpGet("alerts")]
        [ProducesResponseType(typeof(List<Alert>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Alert>>> GetAlertsAsync(CancellationToken cancellationToken)
        {
            var alerts = await _colonyStateGatewayClient.GetAlertsAsync(cancellationToken);
            return Ok(alerts.ToList());
        }

        [HttpPost("alerts/{alertId:guid}/acknowledge")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AcknowledgeAlertAsync(Guid alertId, CancellationToken cancellationToken)
        {
            var acknowledged = await _colonyStateGatewayClient.AcknowledgeAlertAsync(alertId, cancellationToken);

            if (!acknowledged)
                return NotFound();

            return NoContent();
        }

        [HttpGet("tasks")]
        public async Task<ActionResult<List<TaskItem>>> GetActiveTasksAsync(CancellationToken cancellationToken)
        {
            var tasks = await _colonyStateGatewayClient.GetActiveTasksAsync(cancellationToken);
            return Ok(tasks);
        }

        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTaskAsync(TaskItem taskItem, CancellationToken cancellationToken)
        {
            var createdTask = await _colonyStateGatewayClient.CreateTaskAsync(taskItem, cancellationToken);

            return Ok(createdTask);
        }

        [HttpPatch("tasks/status")]
        public async Task<ActionResult<TaskItem>> UpdateTaskStatusAsync(UpdateTaskStatusRequest taskStatusRequest, CancellationToken cancellationToken)
        {
            var updatedTask = await _colonyStateGatewayClient.UpdateTaskStatusAsync(taskStatusRequest, cancellationToken);

            return Ok(updatedTask);
        }
    }
}
