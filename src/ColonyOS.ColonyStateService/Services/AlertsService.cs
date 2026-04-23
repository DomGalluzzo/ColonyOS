using ColonyOS.ColonyStateService.Builders.Alerts;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.Alerts;
using ColonyOS.ColonyStateService.Models;
using ColonyOS.Contracts.Models.Alerts;

namespace ColonyOS.ColonyStateService.Services
{
    public class AlertsService : IAlertsService
    {
        private readonly List<Alert> _alerts = new List<Alert>();
        private const double _oxygenUpperThreshold = 25;
        private const double _powerUpperThreshold = 20;
        private const double _structuralIntegrityUpperThreshold = 30;
        private const double _foodUpperThreshold = 40;
        private const double _waterUpperThreshold = 40;

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
            CreateIfNeeded(
                colonyState.OxygenPercentage < _oxygenUpperThreshold,
                AlertTypeEnum.OxygenCritical,
                AlertSeverityEnum.Critical);

            CreateIfNeeded(
                colonyState.PowerPercentage < _powerUpperThreshold,
                AlertTypeEnum.PowerCritical,
                AlertSeverityEnum.Critical);

            CreateIfNeeded(
                colonyState.StructuralIntegrityPercentage < _structuralIntegrityUpperThreshold,
                AlertTypeEnum.StructuralDamage,
                AlertSeverityEnum.Critical);

            CreateIfNeeded(
                colonyState.FoodPercentage < _foodUpperThreshold,
                AlertTypeEnum.FoodLow,
                AlertSeverityEnum.Critical);

            CreateIfNeeded(
                colonyState.WaterPercentage < _waterUpperThreshold,
                AlertTypeEnum.WaterCritical,
                AlertSeverityEnum.Critical);
        }

        public bool Acknowledge(Guid alertId)
        {
            var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert == null)
                return false;

            alert.Acknowledged = true;
            return true;
        }

        private void CreateIfNeeded(bool condition, AlertTypeEnum alertType, AlertSeverityEnum alertSeverity)
        {
            if (!condition)
                return;

            var existingActiveAlert = _alerts.Any(a => a.Type == alertType);
            if (existingActiveAlert)
                return;

            _alerts.Add(new Alert()
            {
                Id = Guid.NewGuid(),
                Type = alertType,
                Severity = alertSeverity,
                Message = AlertMessageBuilder.Build(alertType),
                CreatedUtc = DateTime.UtcNow,
                Acknowledged = false
            });
        }
    }
}
