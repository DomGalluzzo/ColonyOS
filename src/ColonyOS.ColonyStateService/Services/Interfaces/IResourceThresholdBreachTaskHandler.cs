using ColonyOS.Contracts.Models.Events;

namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface IResourceThresholdBreachTaskHandler
    {
        Task HandleAsync(ResourceThresholdBreachedEvent breachEvent);
    }
}
