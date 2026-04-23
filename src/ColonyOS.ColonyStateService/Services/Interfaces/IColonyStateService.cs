using ColonyOS.ColonyStateService.Models.ColonyState;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface IColonyStateService
    {
        Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken =  default);
    }
}
