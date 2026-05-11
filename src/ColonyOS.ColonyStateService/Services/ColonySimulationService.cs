using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Models.ColonyState.Resources;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.ColonyResources;
using ColonyOS.Contracts.Enums.Tasks;

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
            var utcNow = DateTime.UtcNow;

            foreach (var resource in colonyState.Resources)
            {
                var didResourceTick = ApplyResourceDynamics(resource, utcNow);

                if (didResourceTick)
                    ApplyActiveTaskEffects(resource);
            }

            await Task.CompletedTask;
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

        private void ApplyActiveTaskEffects(ColonyResource resource)
        {
            var activeTasks = _taskService.GetActiveTasks()
                .Where(task =>
                    task.Status == TaskStatusEnum.InProgress &&
                    task.ResourceType == resource.ResourceType &&
                    task.ResourceDeltaPerTick.HasValue);

            foreach (var task in activeTasks)
            {
                resource.Percentage = Math.Clamp(
                    resource.Percentage + task.ResourceDeltaPerTick.Value,
                    0m,
                    100m);
            }
        }
    }
}
