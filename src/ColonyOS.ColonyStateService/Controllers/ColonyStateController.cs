using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.ColonyStateService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ColonyOS.ColonyStateService.Controllers
{
    [ApiController]
    [Route("api/colony-state")]
    public class ColonyStateController : ControllerBase
    {

        private readonly IColonyStateService _colonyStateService;

        public ColonyStateController(IColonyStateService colonyStateService)
        {
            _colonyStateService = colonyStateService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ColonyState), StatusCodes.Status200OK)]
        public async Task<ActionResult<ColonyState>> GetCurrentStateAsync(CancellationToken cancellationToken)
        {
            var state = await _colonyStateService.GetCurrentStateAsync(cancellationToken);
            return Ok(state);
        }
    }
}
