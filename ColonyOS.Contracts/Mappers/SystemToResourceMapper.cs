using ColonyOS.Contracts.Enums.ColonyResources;
using ColonyOS.Contracts.Enums.Target;

namespace ColonyOS.Contracts.Mappers
{
    public static class SystemToResourceMapper
    {
        public static ColonyResourceTypeEnum Map(TargetSystemEnum system)
        {
            return system switch
            {
                TargetSystemEnum.OxygenGenerator => ColonyResourceTypeEnum.Oxygen,
                TargetSystemEnum.WaterRecycler => ColonyResourceTypeEnum.Water,
                TargetSystemEnum.SolarArray => ColonyResourceTypeEnum.Power,
                TargetSystemEnum.FoodProduction => ColonyResourceTypeEnum.Food,
                TargetSystemEnum.HabitatStructure => ColonyResourceTypeEnum.StructuralIntegrity,
                TargetSystemEnum.Radiation => ColonyResourceTypeEnum.Radiation,
                _ => throw new NotImplementedException($"No resource mapping for {system}")
            };
        }
    }
}