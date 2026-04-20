using ColonyOS.ColonyStateService.Models;
using ColonyOS.Contracts.Models.Alerts;

namespace ColonyOS.Gateway.Services
{
    public interface IColonyStateGatewayClient
    {
        Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Alert>> GetAlertsAsync(CancellationToken cancellationToken = default);
        Task<bool> AcknowledgeAlertAsync(Guid alertId, CancellationToken cancellationToken = default);
    }
}
