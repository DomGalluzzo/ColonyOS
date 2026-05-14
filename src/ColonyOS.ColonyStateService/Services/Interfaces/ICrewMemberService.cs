using ColonyOS.Contracts.Models.Crew;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface ICrewMemberService
    {
        List<CrewMember> GetAll();
        bool BeginRecovery(Guid crewId);
    }
}
