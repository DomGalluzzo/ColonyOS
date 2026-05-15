using ColonyOS.Contracts.Models.Requests;
using ColonyOS.Contracts.Enums.Target;
using ColonyOS.Contracts.Models.Tasks;
using ColonyOS.Contracts.Models.Crew;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface ITaskService
    {
        List<TaskItem> GetActiveTasks(CancellationToken cancellationToken = default);
        bool TaskExistsForSystem(TargetSystemEnum targetSystem);
        TaskItem CreateTask(CreateTaskRequest request);
        TaskItem? UpdateTaskStatus(UpdateTaskStatusRequest taskStatusRequest);
        TaskItem? AssignCrewToTask(AssignCrewToTaskRequest request, List<CrewMember> crewMembers);
        TaskItem? CompleteTask(Guid taskId, List<CrewMember> crewMembers);
        bool ReleaseCrewFromTask(Guid taskId, List<CrewMember> crewMembers);
    }
}
