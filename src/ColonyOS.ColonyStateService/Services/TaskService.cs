using System.Threading.Tasks;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.Target;
using ColonyOS.Contracts.Enums.Tasks;
using ColonyOS.Contracts.Mappers;
using ColonyOS.Contracts.Models.Crew;
using ColonyOS.Contracts.Models.Requests;
using ColonyOS.Contracts.Models.Tasks;

namespace ColonyOS.ColonyStateService.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new List<TaskItem>();
        public TaskService()
        {

        }

        public List<TaskItem> GetActiveTasks(CancellationToken cancellationToken = default)
        {
            return _tasks
                .OrderByDescending(t => t.TaskPriority)
                .ThenBy(t => t.CompletedAtUtc)
                .ToList();
        }

        public async Task<bool> TaskExistsForSystemAsync(TargetSystemEnum targetSystem)
        {
            var activeTasks = GetActiveTasks();

            return activeTasks.Any(t => t.TargetSystem == targetSystem &&
                (t.Status == TaskStatusEnum.InProgress || t.Status == TaskStatusEnum.Pending || t.Status == TaskStatusEnum.Assigned));
        }

        public async Task<TaskItem> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
        {
            var newTask = new TaskItem()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                TargetSystem = request.TargetSubsystem,
                TaskPriority = request.TaskPriority,
                TaskType = request.TaskType,
                ResourceType = SystemToResourceMapper.Map(request.TargetSubsystem.Value),
                ResourceDeltaPerTick = 2m,
                Status = TaskStatusEnum.Pending,
                EstimatedDurationMinutes = request.EstimatedDurationMinutes,
                CreatedAtUtc = DateTime.UtcNow
            };

            _tasks.Add(newTask);

            return newTask;
        }

        public async Task<TaskItem?> UpdateTaskStatusAsync(UpdateTaskStatusRequest taskStatusRequest, CancellationToken cancellationToken = default)
        {
            var existingTaskIds = _tasks.Select(t => t.Id);
            if (!existingTaskIds.Contains(taskStatusRequest.TaskId))
                return null;

            var existingTask = _tasks.First(t => t.Id == taskStatusRequest.TaskId);
            existingTask.Status = taskStatusRequest.Status;

            return existingTask;
        }

        public async Task<TaskItem?> AssignCrewToTaskAsync(AssignCrewToTaskRequest request, List<CrewMember> crewMembers, CancellationToken cancellationToken = default)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == request.TaskId);

            if (task == null)
            {
                return null;
            }

            if (task.Status is TaskStatusEnum.Completed or TaskStatusEnum.Failed)
            {
                return null;
            }

            if (task.AssignedCrewMemberId.HasValue)
            {
                return null;
            }

            var crewMember = crewMembers.FirstOrDefault(c => c.Id == request.CrewMemberId);

            if (crewMember == null)
            {
                return null;
            }

            if (!crewMember.IsAvailable || crewMember.CurrentTaskId.HasValue)
            {
                return null;
            }

            task.AssignedCrewMemberId = crewMember.Id;
            task.AssignedAtUtc = DateTime.UtcNow;
            task.Status = TaskStatusEnum.Assigned;

            crewMember.IsAvailable = false;
            crewMember.CurrentTaskId = task.Id;

            return task;
        }
    }
}
