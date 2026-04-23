using ColonyOS.Contracts.Enums.Target;
using ColonyOS.Contracts.Enums.Task;
using ColonyOS.Contracts.Enums.Tasks;

namespace ColonyOS.ColonyStateService.Models.Requests
{
    public class CreateTaskRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public TaskTypeEnum TaskType { get; set; }
        public TargetSystemEnum? TargetSubsystem { get; set; }
        public TaskPriorityEnum Priority { get; set; }
        public int EstimatedDurationMinutes { get; set; }
        public Guid? SourceAlertId { get; set; }
    }
}
