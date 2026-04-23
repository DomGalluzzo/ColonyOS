using ColonyOS.ColonyStateService.Builders.Alerts;
using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Models.ColonyState.Resources;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.Alerts;
using ColonyOS.Contracts.Enums.ColonyResources;
using ColonyOS.Contracts.Models.Alerts;

namespace ColonyOS.ColonyStateService.Services
{
    public class AlertsService : IAlertsService
    {
        private readonly List<Alert> _alerts = new();

        private static readonly IReadOnlyCollection<ResourceAlertRule> _resourceAlertRules =
        [
            new(ColonyResourceTypeEnum.Oxygen, AlertTypeEnum.OxygenCritical, AlertSeverityEnum.Critical),
            new(ColonyResourceTypeEnum.Power, AlertTypeEnum.PowerCritical, AlertSeverityEnum.Critical),
            new(ColonyResourceTypeEnum.StructuralIntegrity, AlertTypeEnum.StructuralDamage, AlertSeverityEnum.Critical),
            new(ColonyResourceTypeEnum.Food, AlertTypeEnum.FoodLow, AlertSeverityEnum.Critical),
            new(ColonyResourceTypeEnum.Water, AlertTypeEnum.WaterCritical, AlertSeverityEnum.Critical)
        ];

        public IReadOnlyCollection<Alert> GetAll()
        {
            return _alerts
                .OrderBy(a => a.Acknowledged)
                .ThenByDescending(a => a.Severity)
                .ThenByDescending(a => a.CreatedUtc)
                .ToList();
        }

        public void EvaluateAlerts(ColonyState colonyState)
        {
            if (colonyState?.Resources == null || colonyState.Resources.Count == 0)
                return;

            foreach (var rule in _resourceAlertRules)
            {
                var colonyStateResource = colonyState.Resources
                    .FirstOrDefault(r => r.ResourceType == rule.ResourceType);

                if (colonyStateResource == null) continue;


                CreateIfNeeded(
                    ShouldTriggerAlert(colonyStateResource),
                    rule.AlertType,
                    rule.AlertSeverity);
            }
        }

        public bool Acknowledge(Guid alertId)
        {
            var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert == null) return false;

            alert.Acknowledged = true;
            return true;
        }

        private bool ShouldTriggerAlert(ColonyResource resource)
        {
            if (resource.MinThreshold.HasValue)
                return resource.Percentage < resource.MinThreshold.Value;

            if (resource.MaxThreshold.HasValue)
                return resource.Percentage > resource.MaxThreshold.Value;

            return false;
        }

        private void CreateIfNeeded(bool condition, AlertTypeEnum alertType, AlertSeverityEnum alertSeverity)
        {
            if (!condition) return;

            var existingActiveAlert = _alerts.Any(a => a.Type == alertType);
            if (existingActiveAlert) return;

            _alerts.Add(new Alert
            {
                Id = Guid.NewGuid(),
                Type = alertType,
                Severity = alertSeverity,
                Message = AlertMessageBuilder.Build(alertType),
                CreatedUtc = DateTime.UtcNow,
                Acknowledged = false
            });
        }

        private sealed record ResourceAlertRule(
            ColonyResourceTypeEnum ResourceType,
            AlertTypeEnum AlertType,
            AlertSeverityEnum AlertSeverity);
    }
}