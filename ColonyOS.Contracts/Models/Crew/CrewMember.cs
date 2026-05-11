using ColonyOS.Contracts.Enums.Crew;

namespace ColonyOS.Contracts.Models.Crew
{
    public class CrewMember
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CrewRoleEnum Role { get; set; }
        public decimal Fatigue { get; set; }
        public bool IsAvailable { get; set; }
        public Guid? CurrentTaskId { get; set; }
        public List<CrewSkillEnum> Skills { get; set; }
    }
}
