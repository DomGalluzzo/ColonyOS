using ColonyOS.ColonyStateService.Models.ColonyState.Resources;

namespace ColonyOS.ColonyStateService.Models.ColonyState
{
    public class ColonyState
    {
        public List<ColonyResource> Resources { get; set; }
        public DateTime LastUpdatedUtc { get; init; }
    }
}
