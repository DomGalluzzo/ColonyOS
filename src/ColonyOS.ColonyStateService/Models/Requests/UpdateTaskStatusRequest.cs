using ColonyOS.Contracts.Enums.Tasks;

namespace ColonyOS.ColonyStateService.Models.Requests
{
    public class UpdateTaskStatusRequest
    {
        public Guid TaskId { get; set; }
        public TaskStatusEnum Status { get; set; }
    }
}
