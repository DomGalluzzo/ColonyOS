using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.ColonyResources;

namespace ColonyOS.ColonyStateService.Services
{
    public class ColonySimulationService : IColonySimulationService
    {
        public async Task ProcessSimulationTickAsync(ColonyState colonyState)
        {
            var utcNow = DateTime.UtcNow;

            foreach (var resource in colonyState.Resources)
            {
                var dynamics = resource.ResourceDynamics;
                if (dynamics.IsPaused) continue;

                var elapsed = utcNow - dynamics.LastTickUtc;
                if (elapsed < dynamics.TickInterval) continue;

                var effectiveRate = dynamics.BaseRatePerTick * dynamics.Modifier;
                var delta = dynamics.Trend == ColonyResourceTrendEnum.Decreasing ? -effectiveRate : effectiveRate;

                resource.Percentage = Math.Clamp(resource.Percentage + delta, 0m, 100m);

                dynamics.LastTickUtc = utcNow;
            }
        }
    }
}
