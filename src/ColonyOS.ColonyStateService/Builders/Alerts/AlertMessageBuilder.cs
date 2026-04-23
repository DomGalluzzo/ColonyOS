using ColonyOS.Contracts.Enums.Alerts;

namespace ColonyOS.ColonyStateService.Builders.Alerts
{
    public static class AlertMessageBuilder
    {
        public static string Build(AlertTypeEnum alertType)
        {
            return alertType switch
            {
                AlertTypeEnum.OxygenCritical => "Oxygen levels critically low",
                AlertTypeEnum.WaterCritical => "Water reserves critically low",
                AlertTypeEnum.PowerCritical => "Power levels unstable",
                AlertTypeEnum.FoodLow => "Food supplies running low",
                AlertTypeEnum.StructuralDamage => "Structural integrity compromised",
                AlertTypeEnum.RadiationCritical => "Radiation levels are critical. Seek shelter Immediately",
                _ => "Unknown alert"
            };
        }
    }
}
