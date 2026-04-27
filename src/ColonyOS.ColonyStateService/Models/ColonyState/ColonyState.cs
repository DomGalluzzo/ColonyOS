using ColonyOS.ColonyStateService.Models.ColonyState.Resources;
using ColonyOS.Contracts.Models.Alerts;
using ColonyOS.Contracts.Models.Tasks;

namespace ColonyOS.ColonyStateService.Models.ColonyState
{
    public class ColonyState
    {
        public List<ColonyResource> Resources { get; set; } = new List<ColonyResource>();
        public List<Alert> Alerts { get; set; } = new List<Alert>();
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public DateTime LastUpdatedUtc { get; set; }
    }
}
