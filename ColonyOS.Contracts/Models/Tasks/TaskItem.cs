using ColonyOS.Contracts.Enums.Target;
using ColonyOS.Contracts.Enums.Task;
using ColonyOS.Contracts.Enums.Tasks;

namespace ColonyOS.Contracts.Models.Tasks
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public TaskTypeEnum TaskType { get; set; }
        public TargetSystemEnum TargetSystem { get; set; }
        public int DurationSeconds { get; set; }
        public TaskStatusEnum Status { get; set; }
    }
}
