using ColonyOS.ColonyStateService.Models.ColonyState;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface IColonySimulationService
    {
        Task ProcessSimulationTickAsync(ColonyState colonyState);
    }
}
