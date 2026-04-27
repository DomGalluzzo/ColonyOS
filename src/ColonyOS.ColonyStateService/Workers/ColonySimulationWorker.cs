using ColonyOS.ColonyStateService.Services.Interfaces;

namespace ColonyOS.ColonyStateService.Workers
{
    public class ColonySimulationWorker : BackgroundService
    {
        private readonly IColonyStateService _colonyStateService;
        private readonly ILogger<ColonySimulationWorker> _logger;

        public ColonySimulationWorker(
            IColonyStateService colonyStateService,
            ILogger<ColonySimulationWorker> logger)
        {
            _colonyStateService = colonyStateService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var simulationTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));

            while (await simulationTimer.WaitForNextTickAsync(cancellationToken))
            {
                try
                {
                    await _colonyStateService.ProcessSimulationTick();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing colony simulation tick.");
                }
            }
        }
    }
}