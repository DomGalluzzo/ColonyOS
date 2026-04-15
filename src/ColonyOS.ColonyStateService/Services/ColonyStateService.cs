using ColonyOS.Contracts.Models;

namespace ColonyOS.ColonyStateService.Services
{
    public class ColonyStateService : IColonyStateService
    {
        public async Task<ColonyStateDto> GetCurrentStateAsync(CancellationToken cancellationToken = default)
        {
            // Temp artificial delay
            await Task.Delay(25, cancellationToken);

            return new ColonyStateDto()
            {
                OxygenPercentage = 97,
                WaterPercentage = 82,
                PowerPercentage = 74,
                FoodPercentage = 91,
                MoralePercentage = 88,
                StructuralIntegrityPercentage = 95,
                LastUpdatedUtc = DateTime.UtcNow
            };
        }
    }
}
