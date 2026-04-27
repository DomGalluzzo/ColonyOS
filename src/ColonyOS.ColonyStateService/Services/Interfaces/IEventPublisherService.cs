namespace ColonyOS.ColonyStateService.Services.Interfaces
{
    public interface IEventPublisherService
    {
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class;
    }
}
