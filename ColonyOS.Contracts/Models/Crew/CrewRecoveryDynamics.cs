namespace ColonyOS.Contracts.Models.Crew
{
    public class CrewRecoveryDynamics
    {
        public decimal RecoveryRatePerTick { get; set; }
        public TimeSpan TickInterval { get; set; }
        public DateTime LastTickUtc { get; set; }
    }
}
