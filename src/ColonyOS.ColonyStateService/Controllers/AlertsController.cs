using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Models.Alerts;
using Microsoft.AspNetCore.Mvc;

namespace ColonyOS.ColonyStateService.Controllers
{
    [ApiController]
    [Route("api/alerts")]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertsService _alertService;

        public AlertsController(IAlertsService alertService)
        {
            _alertService = alertService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<Alert>), StatusCodes.Status200OK)]
        public ActionResult<IReadOnlyCollection<Alert>> GetAll()
        {
            var alerts = _alertService.GetAll();

            return Ok(alerts);
        }

        [HttpPost("{alertId:guid}/acknowledge")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Acknowledge(Guid alertId)
        {
            var acknowledged = _alertService.Acknowledge(alertId);

            if (!acknowledged)
                return NotFound();

            return NoContent();
        }
    }
}
