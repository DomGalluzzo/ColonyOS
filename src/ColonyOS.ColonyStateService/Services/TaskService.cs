using ColonyOS.ColonyStateService.Models.Requests;
using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Enums.Tasks;
using ColonyOS.Contracts.Models.Tasks;

namespace ColonyOS.ColonyStateService.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new List<TaskItem>();
        public TaskService()
        {

        }

        public async Task<IReadOnlyList<TaskItem>> GetActiveTasksAsync(CancellationToken cancellationToken = default)
        {
            return _tasks
                .OrderByDescending(t => t.TaskPriority)
                .ThenBy(t => t.CompletedAtUtc)
                .ToList();
        }
        public async  Task<TaskItem> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
        {
            var newTask = new TaskItem()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                TargetSystem = request.TargetSubsystem,
                TaskPriority = request.TaskPriority,
                TaskType = request.TaskType,
                Status = TaskStatusEnum.Pending,
                EstimatedDurationMinutes = request.EstimatedDurationMinutes,
                CreatedAtUtc = DateTime.UtcNow
            };

            _tasks.Add(newTask);

            return newTask;
        }

        public async Task<TaskItem?> UpdateTaskStatusAsync(Guid taskId, TaskStatusEnum status, CancellationToken cancellationToken = default)
        {
            var existingTaskIds = _tasks.Select(t => t.Id);
            if (!existingTaskIds.Contains(taskId))
                return null;

            var existingTask = _tasks.First(t => t.Id == taskId);
            existingTask.Status = status;

            return existingTask;
        }
    }
}
