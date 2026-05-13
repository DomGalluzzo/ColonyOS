using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.Contracts.Models.Requests;
using ColonyOS.Contracts.Models.Tasks;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface IColonyStateService
    {
        Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken =  default);
        Task ProcessSimulationTick();
        Task<TaskItem?> AssignCrewToTaskAsync(AssignCrewToTaskRequest request, CancellationToken cancellationToken = default);
    }
}
