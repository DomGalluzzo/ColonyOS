using ColonyOS.Contracts.Models;

namespace ColonyOS.ColonyStateService.Services
{
    public interface IColonyStateService
    {
        Task<ColonyStateDto> GetCurrentStateAsync(CancellationToken cancellationToken =  default);
    }
}
