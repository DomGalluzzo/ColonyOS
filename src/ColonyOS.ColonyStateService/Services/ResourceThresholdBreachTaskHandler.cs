using ColonyOS.ColonyStateService.Models.Requests;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.Task;
using ColonyOS.Contracts.Enums.Tasks;
using ColonyOS.Contracts.Models.Events;
using ColonyOS.Contracts.Enums.ColonyResources;

namespace ColonyOS.ColonyStateService.Services
{
    public class ResourceThresholdBreachTaskHandler : IResourceThresholdBreachTaskHandler
    {
        private readonly ITaskService _taskService;

        public ResourceThresholdBreachTaskHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task HandleAsync(ResourceThresholdBreachedEvent breachEvent)
        {
            var existing = await _taskService.TaskExistsForSystemAsync(breachEvent.TargetSystem);
            var systemsReturningToNormal = breachEvent.BreachDirection == ColonyResourceBreachDirectionEnum.Normal;

            if (existing || systemsReturningToNormal)
                return;

            await _taskService.CreateTaskAsync(new CreateTaskRequest
            {
                Title = $"Repair {breachEvent.TargetSystem}",
                Description = $"{breachEvent.ColonyResourceType} at {breachEvent.CurrentPercentage}%",
                TaskPriority = TaskPriorityEnum.Critical,
                TaskType = TaskTypeEnum.Maintenance,
                TargetSubsystem = breachEvent.TargetSystem
            });
        }
    }
}
