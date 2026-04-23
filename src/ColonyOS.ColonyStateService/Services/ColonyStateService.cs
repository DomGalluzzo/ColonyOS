using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.ColonyStateService.Models;

namespace ColonyOS.ColonyStateService.Services
{
    public class ColonyStateService : IColonyStateService
    {
        private readonly IAlertsService _alertsService;
        public ColonyStateService(IAlertsService alertsService)
        {
            _alertsService = alertsService;
        }

        public async Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken = default)
        {
            // Temp artificial delay
            await Task.Delay(25, cancellationToken);

            var colonyState = new ColonyState()
            {
                OxygenPercentage = 22,
                WaterPercentage = 15,
                PowerPercentage = 74,
                FoodPercentage = 91,
                MoralePercentage = 88,
                StructuralIntegrityPercentage = 95,
                LastUpdatedUtc = DateTime.UtcNow
            };

            _alertsService.EvaluateAlerts(colonyState);

            return colonyState;
        }
    }
}
