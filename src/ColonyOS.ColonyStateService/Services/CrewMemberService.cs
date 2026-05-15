using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.Crew;
using ColonyOS.Contracts.Models.Crew;

namespace ColonyOS.ColonyStateService.Services
{
    public class CrewMemberService : ICrewMemberService
    {
        private List<CrewMember> _crewMembers;

        public CrewMemberService()
        {
            _crewMembers = GetInitialCrewMemberState();
        }

        public List<CrewMember> GetAll()
        {
            return _crewMembers;
        }

        public bool BeginRecovery(Guid crewId)
        {
            var crewMember = GetCrewMember(crewId);
            if (crewMember == null || !CanCrewMemberBeginRecovery(crewMember)) return false;

            crewMember.IsAvailable = false;
            crewMember.RecoveryState = CrewRecoveryStateEnum.Recovering;
            crewMember.RecoveryDynamics = new CrewRecoveryDynamics()
            {
                RecoveryRatePerTick = 5,
                TickInterval = TimeSpan.FromSeconds(5),
                LastTickUtc = DateTime.UtcNow
            };

            return true;
        }

        private CrewMember? GetCrewMember(Guid crewId)
        {
            return _crewMembers.FirstOrDefault(crew => crew.Id == crewId);
        }

        private bool CanCrewMemberBeginRecovery(CrewMember crewMember)
        {
            if (crewMember == null) return false;
            if (crewMember.Fatigue <= 0) return false;

            return true;
        }

        private List<CrewMember> GetInitialCrewMemberState()
        {
            return new List<CrewMember>()
            {
                new CrewMember
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Ava Singh",
                    Role = CrewRoleEnum.Engineer,
                    Fatigue = 12,
                    IsAvailable = true,
                    Skills =
                    [
                        CrewSkillEnum.ElectricalRepair,
                        CrewSkillEnum.LifeSupportSystems,
                        CrewSkillEnum.StructuralEngineering
                    ]
                },

                new CrewMember
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Marcus Hale",
                    Role = CrewRoleEnum.Technician,
                    Fatigue = 28,
                    IsAvailable = true,
                    Skills =
                    [
                        CrewSkillEnum.MechanicalRepair,
                        CrewSkillEnum.Robotics,
                        CrewSkillEnum.EmergencyResponse
                    ]
                },

                new CrewMember
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Elena Petrov",
                    Role = CrewRoleEnum.Scientist,
                    Fatigue = 55,
                    IsAvailable = true,
                    Skills =
                    [
                        CrewSkillEnum.ResearchAnalysis,
                        CrewSkillEnum.RadiationMitigation,
                        CrewSkillEnum.SoftwareSystems
                    ]
                },

                new CrewMember
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Noah Brooks",
                    Role = CrewRoleEnum.Medic,
                    Fatigue = 85,
                    IsAvailable = true,
                    Skills =
                    [
                        CrewSkillEnum.MedicalTreatment,
                        CrewSkillEnum.EmergencyResponse
                    ]
                },

                new CrewMember
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Sofia Alvarez",
                    Role = CrewRoleEnum.Botanist,
                    Fatigue = 5,
                    IsAvailable = true,
                    Skills =
                    [
                        CrewSkillEnum.Agriculture,
                        CrewSkillEnum.ResourceOptimization
                    ]
                },

                new CrewMember
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "Daniel Cho",
                    Role = CrewRoleEnum.Commander,
                    Fatigue = 22,
                    IsAvailable = true,
                    Skills =
                    [
                        CrewSkillEnum.EmergencyResponse,
                        CrewSkillEnum.Communications,
                        CrewSkillEnum.Navigation
                    ]
                }
            };
        }
    }
}