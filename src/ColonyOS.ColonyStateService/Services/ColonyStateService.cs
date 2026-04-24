using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Models.ColonyState.Resources;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.ColonyResources;
using ColonyOS.Contracts.Mappers;
using ColonyOS.Contracts.Models.Events;

namespace ColonyOS.ColonyStateService.Services
{
    public class ColonyStateService : IColonyStateService
    {
        private readonly IAlertsService _alertsService;
        private readonly IColonySimulationService _colonySimulationService;
        private readonly IEventPublisherService _eventPublisher;
        private readonly object _lock = new();

        private ColonyState _colonyState;

        public ColonyStateService(IAlertsService alertsService,
            IColonySimulationService colonySimulationService,
            IEventPublisherService eventPublisher)
        {
            _alertsService = alertsService;
            _colonySimulationService = colonySimulationService;
            _eventPublisher = eventPublisher;

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
            ColonyState colonyStateSnaphot;

            lock (_lock)
            {
                _colonySimulationService.ProcessSimulationTickAsync(_colonyState).GetAwaiter().GetResult();
                _colonyState.LastUpdatedUtc = DateTime.UtcNow;
                colonyStateSnaphot = _colonyState;
            }

            await HandleResourceTransitionsAsync(colonyStateSnaphot);

            _alertsService.EvaluateAlerts(colonyStateSnaphot);
        }

        private async Task HandleResourceTransitionsAsync(ColonyState colonyState)
        {
            foreach (var resource in _colonyState.Resources)
            {
                var wasBreached = resource.IsBreached;

                var isBelowMin = resource.MinThreshold.HasValue && resource.Percentage < resource.MinThreshold;
                var isAboveMax = resource.MaxThreshold.HasValue && resource.Percentage > resource.MaxThreshold;

                var isNowBreached = isBelowMin || isAboveMax;

                if (!wasBreached && isNowBreached)
                {
                    resource.IsBreached = true;

                    await _eventPublisher.PublishAsync(new ResourceThresholdBreachedEvent
                    {
                        ColonyResourceType = resource.ResourceType,
                        TargetSystem = ResourceToSystemMapper.Map(resource.ResourceType),
                        CurrentPercentage = resource.Percentage,
                        MinThreshold = resource.MinThreshold,
                        MaxThreshold = resource.MaxThreshold,
                        BreachDirection = isBelowMin
                            ? ColonyResourceBreachDirectionEnum.BelowMinimum
                            : ColonyResourceBreachDirectionEnum.AboveMaximum,
                        OccurredAtUtc = DateTime.UtcNow
                    });
                }
                else if (wasBreached && !isNowBreached)
                {
                    resource.IsBreached = false;

                    await _eventPublisher.PublishAsync(new ResourceThresholdBreachedEvent
                    {
                        ColonyResourceType = resource.ResourceType,
                        TargetSystem = ResourceToSystemMapper.Map(resource.ResourceType),
                        CurrentPercentage = resource.Percentage,
                        MinThreshold = resource.MinThreshold,
                        MaxThreshold = resource.MaxThreshold,
                        BreachDirection = ColonyResourceBreachDirectionEnum.Normal,
                        OccurredAtUtc = DateTime.UtcNow
                    });
                }
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
                        Percentage = 31,
                        MinThreshold = 30,
                        ResourceType = ColonyResourceTypeEnum.Oxygen,
                        ResourceDynamics = new ResourceDynamics()
                        {
                            BaseRatePerTick = 0.25m,
                            Trend = ColonyResourceTrendEnum.Decreasing,
                            TickInterval = TimeSpan.FromSeconds(15)
                        }
                    },
                    new ColonyResource()
                    {
                        Title = "Water",
                        Percentage = 42,
                        MinThreshold = 40,
                        ResourceType = ColonyResourceTypeEnum.Water,
                        ResourceDynamics = new ResourceDynamics()
                        {
                            BaseRatePerTick = 0.10m,
                            Trend = ColonyResourceTrendEnum.Decreasing,
                            TickInterval = TimeSpan.FromSeconds(5)
                        }
                    },
                    new ColonyResource()
                    {
                        Title = "Power",
                        Percentage = 20,
                        MinThreshold = 20,
                        ResourceType = ColonyResourceTypeEnum.Power,
                        ResourceDynamics = new ResourceDynamics()
                        {
                            BaseRatePerTick = 0.01m,
                            Trend = ColonyResourceTrendEnum.Decreasing,
                            TickInterval = TimeSpan.FromSeconds(30)
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
                            Trend = ColonyResourceTrendEnum.Decreasing,
                            TickInterval = TimeSpan.FromSeconds(2)
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
                            Trend = ColonyResourceTrendEnum.Decreasing,
                            TickInterval = TimeSpan.FromSeconds(35)
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
                            Trend = ColonyResourceTrendEnum.Decreasing,
                            TickInterval = TimeSpan.FromSeconds(20)
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
                            Trend = ColonyResourceTrendEnum.Increasing,
                            TickInterval = TimeSpan.FromMinutes(2)
                        }
                    }
                ],
                LastUpdatedUtc = DateTime.UtcNow
            };
        }
    }
}
