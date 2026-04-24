using ColonyOS.ColonyStateService.Models.ColonyState.Resources;

namespace ColonyOS.ColonyStateService.Models.ColonyState
{
    public class ColonyState
    {
        public List<ColonyResource> Resources { get; set; } = new List<ColonyResource>();
        public DateTime LastUpdatedUtc { get; set; }
    }
}
