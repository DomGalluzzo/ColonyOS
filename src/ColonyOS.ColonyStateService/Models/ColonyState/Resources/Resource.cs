using ColonyOS.Contracts.Enums.ColonyResources;

namespace ColonyOS.ColonyStateService.Models.ColonyState.Resources
{
    public class ColonyResource
    {
        public ColonyResourceTypeEnum ResourceType { get; set; }
        public string Title { get; set; }
        public decimal Percentage { get; set; }
        public decimal? MinThreshold { get; set; }
        public decimal? MaxThreshold { get; set; }
        public bool IsBreached { get; set; }
        public ResourceDynamics ResourceDynamics { get; set; }
    }
}
