using ColonyOS.ColonyStateService.Models.Requests;
using ColonyOS.Contracts.Enums.Target;
using ColonyOS.Contracts.Enums.Tasks;
using ColonyOS.Contracts.Models.Tasks;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IReadOnlyList<TaskItem>> GetActiveTasksAsync(CancellationToken cancellationToken = default);
        Task<bool> TaskExistsForSystemAsync(TargetSystemEnum targetSystem);
        Task<TaskItem> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default);
        Task<TaskItem?> UpdateTaskStatusAsync(Guid taskId, TaskStatusEnum status, CancellationToken cancellationToken = default);
    }
}
