using ColonyOS.Contracts.Models;

namespace ColonyOS.Gateway.Services
{
    public interface IColonyStateGatewayClient
    {
        Task<ColonyStateDto> GetCurrentStateAsync(CancellationToken cancellationToken = default);
    }
}
