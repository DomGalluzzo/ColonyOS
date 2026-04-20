using ColonyOS.Contracts.Enums.Target;
using ColonyOS.Contracts.Enums.Task;
using ColonyOS.Contracts.Enums.Tasks;

namespace ColonyOS.Contracts.Models.Tasks
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskTypeEnum TaskType { get; set; }
        public TargetSystemEnum? TargetSystem { get; set; }
        public TaskPriorityEnum TaskPriority { get; set; }
        public TaskStatusEnum Status { get; set; }
        public int EsitmatedDurationMinutes { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? StartedAtUtc { get; set; }
        public DateTime? CompletedAtUtc { get; set; }
        public Guid? SourceAlertId { get; set; }
    }
}
