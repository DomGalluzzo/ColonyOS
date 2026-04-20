using ColonyOS.Contracts.Enums.Alerts;
using ColonyOS.Contracts.Enums.Target;

namespace ColonyOS.Contracts.Models.Alerts
{
    public class Alert
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public AlertTypeEnum Type { get; set; }
        public AlertSeverityEnum Severity { get; set; }
        public TargetSystemEnum TargetSystem { get; set; }
        public bool Acknowledged { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? AcknowledgedAtUtc { get; set; }
        public Guid? RelatedTaskId { get; set; }
    }
}
