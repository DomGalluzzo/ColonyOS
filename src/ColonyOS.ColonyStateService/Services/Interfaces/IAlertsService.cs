using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.Contracts.Models.Alerts;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface IAlertsService
    {
        IReadOnlyCollection<Alert> GetAll();
        void EvaluateAlerts(ColonyState colonyState);
        bool Acknowledge(Guid id);
    }
}
