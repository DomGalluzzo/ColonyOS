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

        [HttpGet("state")]
        [ProducesResponseType(typeof(ColonyState), StatusCodes.Status200OK)]
        public async Task<ActionResult<ColonyState>> GetStateAsync(CancellationToken cancellationToken)
        {
            var state = await _colonyStateGatewayClient.GetCurrentStateAsync(cancellationToken);
            return Ok(state);
        }
    }
}
