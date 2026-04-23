using ColonyOS.Contracts.Enums.ColonyResources;

namespace ColonyOS.ColonyStateService.Models.ColonyState.Resources
{
    public class ColonyResource
    {
        public ColonyResourceTypeEnum ResourceType { get; set; }
        public string Title { get; set; }
        public double Percentage { get; set; }
        public double? MinThreshold { get; set; }
        public double? MaxThreshold { get; set; }
    }
}
