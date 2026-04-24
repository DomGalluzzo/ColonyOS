using ColonyOS.Contracts.Enums.ColonyResources;
using ColonyOS.Contracts.Enums.Target;

namespace ColonyOS.Contracts.Models.Events
{
    public class ResourceThresholdBreachedEvent
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public ColonyResourceTypeEnum ColonyResourceType { get; set; }
        public TargetSystemEnum TargetSystem { get; set; }
        public decimal CurrentPercentage { get; set; }
        public decimal? MinThreshold { get; set; }
        public decimal? MaxThreshold { get; set; }
        public bool IsNewBreach { get; set; }
        public ColonyResourceBreachDirectionEnum BreachDirection { get; set; }
        public DateTime OccurredAtUtc { get; set; }
    }
}
