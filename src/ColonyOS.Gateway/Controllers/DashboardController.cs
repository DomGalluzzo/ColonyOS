using ColonyOS.Contracts.Models;
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

        [HttpGet("State")]
        [ProducesResponseType(typeof(ColonyStateDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ColonyStateDto>> GetStateAsync(CancellationToken cancellationToken)
        {
            var state = await _colonyStateGatewayClient.GetCurrentStateAsync(cancellationToken);
            return Ok(state);
        }
    }
}
