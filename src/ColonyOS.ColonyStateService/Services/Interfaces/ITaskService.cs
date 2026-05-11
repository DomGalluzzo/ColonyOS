using ColonyOS.Contracts.Models.Requests;
using ColonyOS.Contracts.Enums.Target;
using ColonyOS.Contracts.Models.Tasks;
using ColonyOS.Contracts.Models.Crew;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface ITaskService
    {
        List<TaskItem> GetActiveTasks(CancellationToken cancellationToken = default);
        Task<bool> TaskExistsForSystemAsync(TargetSystemEnum targetSystem);
        Task<TaskItem> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default);
        Task<TaskItem?> UpdateTaskStatusAsync(UpdateTaskStatusRequest taskStatusRequest, CancellationToken cancellationToken = default);
        Task<TaskItem?> AssignCrewToTaskAsync(AssignCrewToTaskRequest request, List<CrewMember> crewMembers, CancellationToken cancellationToken = default);
    }
}
