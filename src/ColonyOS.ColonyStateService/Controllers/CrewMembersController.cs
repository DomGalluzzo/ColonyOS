using ColonyOS.ColonyStateService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ColonyOS.ColonyStateService.Controllers
{
    [ApiController]
    [Route("api/crew-members")]
    public class CrewMembersController : ControllerBase
    {
        private readonly ICrewMemberService _crewMemberService;

        public CrewMembersController(ICrewMemberService crewMemberService)
        {
            _crewMemberService = crewMemberService;
        }

        [HttpPost("{crewId:guid}/begin-recovery")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BeginRecovery(Guid crewId)
        {
            var recoveryBegan = _crewMemberService.BeginRecovery(crewId);

            if (!recoveryBegan)
                return NotFound();

            return NoContent();
        }
    }
}
