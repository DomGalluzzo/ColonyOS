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

        public bool TaskExistsForSystem(TargetSystemEnum targetSystem)
        {
            var activeTasks = GetActiveTasks();

            return activeTasks.Any(t => t.TargetSystem == targetSystem &&
                (t.Status == TaskStatusEnum.InProgress || t.Status == TaskStatusEnum.Pending || t.Status == TaskStatusEnum.Assigned));
        }

        public TaskItem CreateTask(CreateTaskRequest request)
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

        public TaskItem? UpdateTaskStatus(UpdateTaskStatusRequest taskStatusRequest)
        {
            var existingTaskIds = _tasks.Select(t => t.Id);
            if (!existingTaskIds.Contains(taskStatusRequest.TaskId))
                return null;

            var existingTask = _tasks.First(t => t.Id == taskStatusRequest.TaskId);
            existingTask.Status = taskStatusRequest.Status;

            return existingTask;
        }

        public TaskItem? AssignCrewToTask(AssignCrewToTaskRequest request, List<CrewMember> crewMembers)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == request.TaskId);

            if (task == null)
                return null;

            if (task.Status is TaskStatusEnum.Completed or TaskStatusEnum.Failed)
                return null;

            if (task.AssignedCrewMemberId.HasValue)
                ReleaseCrewFromTask(request.TaskId, crewMembers);

            var crewMember = crewMembers.FirstOrDefault(c => c.Id == request.CrewMemberId);

            if (crewMember == null)
                return null;

            if (!crewMember.IsAvailable || crewMember.CurrentTaskId.HasValue)
                return null;

            task.AssignedCrewMemberId = crewMember.Id;
            task.AssignedAtUtc = DateTime.UtcNow;
            task.Status = TaskStatusEnum.InProgress;

            crewMember.IsAvailable = false;
            crewMember.CurrentTaskId = task.Id;

            return task;
        }

        public TaskItem? CompleteTask(Guid taskId, List<CrewMember> crewMembers)
        {
            var task = _tasks.FirstOrDefault(task => task.Id == taskId);
            if (task == null) return null;

            task.Status = TaskStatusEnum.Completed;
            task.CompletedAtUtc = DateTime.UtcNow;

            if (task.AssignedCrewMemberId.HasValue)
            {
                var crewMember = crewMembers.FirstOrDefault(crew => crew.Id == task.AssignedCrewMemberId.Value);

                if (crewMember != null)
                {
                    crewMember.IsAvailable = true;
                    crewMember.CurrentTaskId = null;
                }
            }

            return task;
        }

        public bool ReleaseCrewFromTask(Guid taskId, List<CrewMember> crewMembers)
        {
            var task = _tasks.FirstOrDefault(task => task.Id == taskId);

            if (task == null) return false;
            if (!task.AssignedCrewMemberId.HasValue) return false;

            var crewMember = crewMembers.FirstOrDefault(crew => crew.Id == task.AssignedCrewMemberId.Value);

            if (crewMember != null)
                crewMember.IsAvailable = true;

            task.AssignedCrewMemberId = null;

            return true;
        }
    }
}
