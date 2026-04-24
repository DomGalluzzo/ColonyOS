using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Models.ColonyState.Resources;
using ColonyOS.Contracts.Enums.ColonyResources;

namespace ColonyOS.ColonyStateService.Services
{
    public class ColonyStateService : IColonyStateService
    {
        private readonly IAlertsService _alertsService;
        private readonly IColonySimulationService _colonySimulationService;
        private readonly object _lock = new();

        private ColonyState _colonyState;

        public ColonyStateService(IAlertsService alertsService,
            IColonySimulationService colonySimulationService)
        {
            _alertsService = alertsService;
            _colonySimulationService = colonySimulationService;
            _colonyState = GetHardcodedColonyState();
        }

        public async Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken = default)
        {
            // Temp artificial delay
            await Task.Delay(25, cancellationToken);

            lock (_lock)
            {
                _alertsService.EvaluateAlerts(_colonyState);
                return _colonyState;
            }
        }

        public async Task ProcessSimulationTick()
        {
            lock (_lock)
            {
                _colonySimulationService.ProcessSimulationTickAsync(_colonyState).GetAwaiter().GetResult();
                _colonyState.LastUpdatedUtc = DateTime.UtcNow;
                _alertsService.EvaluateAlerts(_colonyState);
            }
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
                        Percentage = 100,
                        MinThreshold = 30,
                        ResourceType = ColonyResourceTypeEnum.Oxygen,
                        ResourceDynamics = new ResourceDynamics()
                        {
                            BaseRatePerTick = 0.25m,
                            Trend = ColonyResourceTrendEnum.Decreasing
                        }
                    },
                    new ColonyResource()
                    {
                        Title = "Water",
                        Percentage = 100,
                        MinThreshold = 40,
                        ResourceType = ColonyResourceTypeEnum.Water,
                        ResourceDynamics = new ResourceDynamics()
                        {
                            BaseRatePerTick = 0.10m,
                            Trend = ColonyResourceTrendEnum.Decreasing
                        }
                    },
                    new ColonyResource()
                    {
                        Title = "Power",
                        Percentage = 100,
                        MinThreshold = 20,
                        ResourceType = ColonyResourceTypeEnum.Power,
                        ResourceDynamics = new ResourceDynamics()
                        {
                            BaseRatePerTick = 0.01m,
                            Trend = ColonyResourceTrendEnum.Decreasing
                        }
                    },
                    new ColonyResource()
                    {
                        Title = "Food",
                        Percentage = 100,
                        MinThreshold = 40,
                        ResourceType = ColonyResourceTypeEnum.Food,
                        ResourceDynamics = new ResourceDynamics()
                        {
                            BaseRatePerTick = 0.09m,
                            Trend = ColonyResourceTrendEnum.Decreasing
                        }
                    },
                    new ColonyResource()
                    {
                        Title = "Morale",
                        Percentage = 100,
                        MinThreshold = 50,
                        ResourceType = ColonyResourceTypeEnum.Morale,
                        ResourceDynamics = new ResourceDynamics()
                        {
                            BaseRatePerTick = 0.09m,
                            Trend = ColonyResourceTrendEnum.Decreasing
                        }
                    },
                    new ColonyResource()
                    {
                        Title = "Structural Integrity",
                        Percentage = 100,
                        MinThreshold = 30,
                        ResourceType = ColonyResourceTypeEnum.StructuralIntegrity,
                        ResourceDynamics = new ResourceDynamics()
                        {
                            BaseRatePerTick = 0.2m,
                            Trend = ColonyResourceTrendEnum.Decreasing
                        }
                    },
                    new ColonyResource()
                    {
                        Title = "Radiation",
                        Percentage = 0,
                        MaxThreshold = 55,
                        ResourceType = ColonyResourceTypeEnum.Radiation,
                        ResourceDynamics = new ResourceDynamics()
                        {
                            BaseRatePerTick = 0.05m,
                            Trend = ColonyResourceTrendEnum.Increasing
                        }
                    }
                ],
                LastUpdatedUtc = DateTime.UtcNow
            };
        }
    }
}
