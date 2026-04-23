using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Models.ColonyState.Resources;
using ColonyOS.Contracts.Enums.ColonyResources;

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

            var colonyState = GetHardcodedColonyState();

            _alertsService.EvaluateAlerts(colonyState);

            return colonyState;
        }

        private ColonyState GetHardcodedColonyState()
        {
            return new ColonyState
            {
                Resources =
                [
                    new ColonyResource()
                    {
                        Title = "Oxygen",
                        Percentage = 22,
                        MinThreshold = 30,
                        ResourceType = ColonyResourceTypeEnum.Oxygen
                    },
                    new ColonyResource()
                    {
                        Title = "Water",
                        Percentage = 15,
                        MinThreshold = 40,
                        ResourceType = ColonyResourceTypeEnum.Water
                    },
                    new ColonyResource()
                    {
                        Title = "Power",
                        Percentage = 74,
                        MinThreshold = 20,
                        ResourceType = ColonyResourceTypeEnum.Power
                    },
                    new ColonyResource()
                    {
                        Title = "Food",
                        Percentage = 91,
                        MinThreshold = 40,
                        ResourceType = ColonyResourceTypeEnum.Food
                    },
                    new ColonyResource()
                    {
                        Title = "Morale",
                        Percentage = 55,
                        MinThreshold = 50,
                        ResourceType = ColonyResourceTypeEnum.Morale
                    },
                    new ColonyResource()
                    {
                        Title = "Structural Integrity",
                        Percentage = 95,
                        MinThreshold = 30,
                        ResourceType = ColonyResourceTypeEnum.StructuralIntegrity
                    },
                    new ColonyResource()
                    {
                        Title = "Radiation",
                        Percentage = 56,
                        MaxThreshold = 55,
                        ResourceType = ColonyResourceTypeEnum.Radiation
                    }
                ],
                LastUpdatedUtc = DateTime.UtcNow
            };
        }
    }
}
