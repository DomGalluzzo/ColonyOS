using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.ColonyResources;

namespace ColonyOS.ColonyStateService.Services
{
    public class ColonySimulationService : IColonySimulationService
    {
        public async Task ProcessSimulationTickAsync(ColonyState colonyState)
        {
            foreach (var resource in colonyState.Resources)
            {
                var dynamics = resource.ResourceDynamics;
                var delta = dynamics.Trend == ColonyResourceTrendEnum.Decreasing ? -dynamics.BaseRatePerTick : dynamics.BaseRatePerTick;

                resource.Percentage = Math.Clamp(resource.Percentage + delta, 0, 100);
            }
        }
    }
}
