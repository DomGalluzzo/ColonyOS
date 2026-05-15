using ColonyOS.ColonyStateService.Models.ColonyState;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface IColonySimulationService
    {
        void ProcessSimulationTick(ColonyState colonyState);
    }
}
