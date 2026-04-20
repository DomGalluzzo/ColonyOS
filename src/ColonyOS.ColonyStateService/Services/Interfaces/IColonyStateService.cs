using ColonyOS.ColonyStateService.Models;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface IColonyStateService
    {
        Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken =  default);
    }
}
