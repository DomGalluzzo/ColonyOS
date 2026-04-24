using ColonyOS.Contracts.Enums.ColonyResources;

namespace ColonyOS.ColonyStateService.Models.ColonyState.Resources
{
    public class ResourceDynamics
    {
        public decimal BaseRatePerTick { get; set; }
        public ColonyResourceTrendEnum Trend { get; set; }
        public decimal Modifier { get; set; } = 1.0m;
        public bool IsPaused { get; set; }
        public TimeSpan TickInterval { get; set; } = TimeSpan.FromSeconds(5);
        public DateTime LastTickUtc { get; set; } = DateTime.UtcNow;
    }
}
