namespace ColonyOS.Contracts.Models.Requests
{
    public class AssignCrewToTaskRequest
    {
        public Guid TaskId { get; set; }
        public Guid CrewMemberId { get; set; }
    }
}
