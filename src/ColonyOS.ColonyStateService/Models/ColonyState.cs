namespace ColonyOS.ColonyStateService.Models
{
    public class ColonyState
    {
        public double OxygenPercentage { get; init; }
        public double WaterPercentage { get; init; }
        public double PowerPercentage { get; init; }
        public double FoodPercentage { get; init; }
        public double MoralePercentage { get; init; }
        public double StructuralIntegrityPercentage { get; init; }
        public DateTime LastUpdatedUtc { get; init; }
    }
}
