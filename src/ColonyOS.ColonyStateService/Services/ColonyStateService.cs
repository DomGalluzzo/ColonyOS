using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Models.ColonyState.Resources;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.ColonyResources;
using ColonyOS.Contracts.Enums.Crew;
using ColonyOS.Contracts.Mappers;
using ColonyOS.Contracts.Models.Crew;
using ColonyOS.Contracts.Models.Events;
using ColonyOS.Contracts.Models.Requests;
using ColonyOS.Contracts.Models.Tasks;

namespace ColonyOS.ColonyStateService.Services
{
    public class ColonyStateService : IColonyStateService
    {
        private readonly IAlertsService _alertsService;
        private readonly ITaskService _taskService;
        private readonly IColonySimulationService _colonySimulationService;
        private readonly IEventPublisherService _eventPublisher;
        private readonly object _lock = new();

        private ColonyState _colonyState;

        public ColonyStateService(IAlertsService alertsService,
            ITaskService taskService,
            IColonySimulationService colonySimulationService,
            IEventPublisherService eventPublisher)
        {
            _alertsService = alertsService;
            _taskService = taskService;
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
                HydrateState();
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

                HydrateState();

                colonyStateSnaphot = _colonyState;
            }

            await HandleResourceTransitionsAsync(colonyStateSnaphot);

            _alertsService.EvaluateAlerts(colonyStateSnaphot);
        }

        public async Task<TaskItem?> AssignCrewToTaskAsync(AssignCrewToTaskRequest request, CancellationToken cancellationToken = default)
        {
            lock (_lock)
            {
                var assignedTask = _taskService.AssignCrewToTaskAsync(request, _colonyState.CrewMembers, cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                HydrateState();

                return assignedTask;
            }
        }

        private void HydrateState()
        {
            _colonyState.Alerts = _alertsService.GetAll().ToList();
            _colonyState.Tasks = _taskService.GetActiveTasks();
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
                CrewMembers =
                [
                    new CrewMember
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Ava Singh",
                        Role = CrewRoleEnum.Engineer,
                        Fatigue = 12,
                        IsAvailable = true,
                        Skills =
                        [
                            CrewSkillEnum.ElectricalRepair,
                            CrewSkillEnum.LifeSupportSystems,
                            CrewSkillEnum.StructuralEngineering
                        ]
                    },

                    new CrewMember
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Marcus Hale",
                        Role = CrewRoleEnum.Technician,
                        Fatigue = 28,
                        IsAvailable = true,
                        Skills =
                        [
                            CrewSkillEnum.MechanicalRepair,
                            CrewSkillEnum.Robotics,
                            CrewSkillEnum.EmergencyResponse
                        ]
                    },

                    new CrewMember
                    {
                        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Name = "Elena Petrov",
                        Role = CrewRoleEnum.Scientist,
                        Fatigue = 8,
                        IsAvailable = true,
                        Skills =
                        [
                            CrewSkillEnum.ResearchAnalysis,
                            CrewSkillEnum.RadiationMitigation,
                            CrewSkillEnum.SoftwareSystems
                        ]
                    },

                    new CrewMember
                    {
                        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        Name = "Noah Brooks",
                        Role = CrewRoleEnum.Medic,
                        Fatigue = 18,
                        IsAvailable = true,
                        Skills =
                        [
                            CrewSkillEnum.MedicalTreatment,
                            CrewSkillEnum.EmergencyResponse
                        ]
                    },

                    new CrewMember
                    {
                        Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                        Name = "Sofia Alvarez",
                        Role = CrewRoleEnum.Botanist,
                        Fatigue = 5,
                        IsAvailable = true,
                        Skills =
                        [
                            CrewSkillEnum.Agriculture,
                            CrewSkillEnum.ResourceOptimization
                        ]
                    },

                    new CrewMember
                    {
                        Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                        Name = "Daniel Cho",
                        Role = CrewRoleEnum.Commander,
                        Fatigue = 22,
                        IsAvailable = true,
                        Skills =
                        [
                            CrewSkillEnum.EmergencyResponse,
                            CrewSkillEnum.Communications,
                            CrewSkillEnum.Navigation
                        ]
                    }
                ],
                LastUpdatedUtc = DateTime.UtcNow
            };
        }
    }
}
