using ColonyOS.ColonyStateService.Models;
using ColonyOS.Contracts.Models.Alerts;
using ColonyOS.Contracts.Models.Tasks;

namespace ColonyOS.Gateway.Services
{
    public interface IColonyStateGatewayClient
    {
        Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Alert>> GetAlertsAsync(CancellationToken cancellationToken = default);
        Task<bool> AcknowledgeAlertAsync(Guid alertId, CancellationToken cancellationToken = default);
        Task<TaskItem> CreateTaskAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
        Task<List<TaskItem>> GetActiveTasksAsync(CancellationToken cancellationToken = default);
    }
}
