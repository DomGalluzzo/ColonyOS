using ColonyOS.Contracts.Enums.Alerts;

namespace ColonyOS.Contracts.Models.Alerts
{
    public class Alert
    {
        public Guid Id { get; set; }
        public AlertTypeEnum Type { get; set; }
        public AlertSeverityEnum Severity { get; set; }
        public string Message { get; set; }
        public DateTime CreatedUtc { get; set; }
        public bool Acknowledged { get; set; }
    }
}
