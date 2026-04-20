using ColonyOS.Contracts.Enums.Tasks;

namespace ColonyOS.ColonyStateService.Models.Requests
{
    public class UpdateTaskStatusRequest
    {
        public TaskStatusEnum Status { get; set; }
    }
}
