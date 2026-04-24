using ColonyOS.ColonyStateService.Services.Interfaces;
using ColonyOS.Contracts.Models.Events;

namespace ColonyOS.ColonyStateService.Services
{
    public class EventPublisherService : IEventPublisherService
    {
        private readonly IServiceProvider _serviceProvider;

        public EventPublisherService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class
        {
            switch (@event)
            {
                case ResourceThresholdBreachedEvent resourceThresholdBreachedEvent:
                    var handler = _serviceProvider.GetRequiredService<IResourceThresholdBreachTaskHandler>();
                    await handler.HandleAsync(resourceThresholdBreachedEvent);
                    break;

                default:
                    throw new NotSupportedException($"No handler registered for event type {typeof(TEvent).Name}");
            }
        }
    }
}
