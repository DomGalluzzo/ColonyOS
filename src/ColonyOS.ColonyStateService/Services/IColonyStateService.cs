using ColonyOS.Contracts.Models;

namespace ColonyOS.ColonyStateService.Services
{
    public interface IColonyStateService
    {
        Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken =  default);
    }
}
