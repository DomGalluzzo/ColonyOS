using ColonyOS.Contracts.Enums.ColonyResources;
using ColonyOS.Contracts.Enums.Target;

namespace ColonyOS.Contracts.Mappers
{
    public static class ResourceToSystemMapper
    {
        public static TargetSystemEnum Map(ColonyResourceTypeEnum resource)
        {
            return resource switch
            {
                ColonyResourceTypeEnum.Oxygen => TargetSystemEnum.OxygenGenerator,
                ColonyResourceTypeEnum.Water => TargetSystemEnum.WaterRecycler,
                ColonyResourceTypeEnum.Power => TargetSystemEnum.SolarArray,
                ColonyResourceTypeEnum.Food => TargetSystemEnum.FoodProduction,
                ColonyResourceTypeEnum.StructuralIntegrity => TargetSystemEnum.HabitatStructure,
                _ => throw new NotImplementedException($"No system mapping for {resource}")
            };
        }
    }
}
