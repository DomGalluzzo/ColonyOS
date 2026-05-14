using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Models.ColonyState.Resources;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.ColonyResources;
using ColonyOS.Contracts.Enums.Crew;
using ColonyOS.Contracts.Enums.Tasks;
using ColonyOS.Contracts.Models.Crew;

namespace ColonyOS.ColonyStateService.Services
{
    public class ColonySimulationService : IColonySimulationService
    {
        private readonly ITaskService _taskService;

        public ColonySimulationService(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task ProcessSimulationTickAsync(ColonyState colonyState)
        {
            ProcessResourceTicks(colonyState);
            ProcessCrewRecoveryTicks(colonyState);

            await Task.CompletedTask;
        }

        private void ProcessResourceTicks(ColonyState colonyState)
        {
            var utcNow = DateTime.UtcNow;

            foreach (var resource in colonyState.Resources)
            {
                var didResourceTick = ApplyResourceDynamics(resource, utcNow);

                if (didResourceTick)
                    ApplyActiveTaskEffects(resource, colonyState);
            }
        }

        private void ProcessCrewRecoveryTicks(ColonyState colonyState)
        {
            var utcNow = DateTime.UtcNow;

            foreach (var crewMember in colonyState.CrewMembers.Where(crew => crew.RecoveryState == CrewRecoveryStateEnum.Recovering))
            {
                var dynamics = crewMember.RecoveryDynamics;
                if (dynamics == null) continue;

                var nextTickUtc = dynamics.LastTickUtc + dynamics.TickInterval;

                if (utcNow < nextTickUtc) continue;

                crewMember.Fatigue = Math.Max(0, crewMember.Fatigue - dynamics.RecoveryRatePerTick);
                dynamics.LastTickUtc = utcNow;

                if (crewMember.Fatigue == 0)
                {
                    crewMember.IsAvailable = true;
                    crewMember.RecoveryState = CrewRecoveryStateEnum.None;
                    crewMember.RecoveryDynamics = null;
                }
            }
        }

        private static bool ApplyResourceDynamics(ColonyResource resource, DateTime utcNow)
        {
            var dynamics = resource.ResourceDynamics;

            if (dynamics.IsPaused)
                return false;

            var elapsed = utcNow - dynamics.LastTickUtc;

            if (elapsed < dynamics.TickInterval)
                return false;

            var effectiveRate = dynamics.BaseRatePerTick * dynamics.Modifier;
            var delta = dynamics.Trend == ColonyResourceTrendEnum.Decreasing
                ? -effectiveRate
                : effectiveRate;

            resource.Percentage = Math.Clamp(resource.Percentage + delta, 0m, 100m);

            dynamics.LastTickUtc = utcNow;

            return true;
        }

        private void ApplyActiveTaskEffects(ColonyResource resource, ColonyState colonyState)
        {
            var activeTasks = _taskService.GetActiveTasks()
                .Where(task =>
                    task.Status == TaskStatusEnum.InProgress &&
                    task.ResourceType == resource.ResourceType &&
                    task.ResourceDeltaPerTick.HasValue);

            foreach (var task in activeTasks)
            {
                var taskDelta = resource.ResourceType == ColonyResourceTypeEnum.Radiation
                    ? -task.ResourceDeltaPerTick.Value
                    : task.ResourceDeltaPerTick.Value;


                resource.Percentage = Math.Clamp(
                    resource.Percentage + taskDelta,
                    0m,
                    100m);

                if (!IsResourceFullyRepaired(resource)) continue;

                resource.Percentage = GetFullyRepairedPercentage(resource);
                _taskService.CompleteTask(task.Id, colonyState.CrewMembers);
            }
        }

        private static bool IsResourceFullyRepaired(ColonyResource resource)
        {
            return resource.ResourceType == ColonyResourceTypeEnum.Radiation
                ? resource.Percentage <= 0m
                : resource.Percentage >= 100m;
        }

        private static decimal GetFullyRepairedPercentage(ColonyResource resource)
        {
            return resource.ResourceType == ColonyResourceTypeEnum.Radiation
                ? 0m
                : 100m;
        }
    }
}
