using ColonyOS.Contracts.Models;

namespace ColonyOS.Gateway.Services
{
    public interface IColonyStateGatewayClient
    {
        Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken = default);
    }
}
