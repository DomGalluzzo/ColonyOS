using ColonyOS.Gateway.Hubs;
using ColonyOS.Gateway.Services;
using Microsoft.AspNetCore.SignalR;

namespace ColonyOS.Gateway.Workers
{
    public class ColonyDashboardBroadcastWorker : BackgroundService
    {
        private readonly IColonyStateGatewayClient _colonyStateGatewayClient;
        private readonly IHubContext<ColonyDashboardHub> _hubContext;
        private readonly ILogger<ColonyDashboardBroadcastWorker> _logger;

        public ColonyDashboardBroadcastWorker(IColonyStateGatewayClient colonyStateGatewayClient,
            IHubContext<ColonyDashboardHub> hubContext,
            ILogger<ColonyDashboardBroadcastWorker> logger)
        {
            _colonyStateGatewayClient = colonyStateGatewayClient;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var broadcastTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));

            while (await broadcastTimer.WaitForNextTickAsync(cancellationToken))
            {
                try
                {
                    var state = await _colonyStateGatewayClient.GetCurrentStateAsync(cancellationToken);

                    await _hubContext.Clients.All.SendAsync(
                        "ColonyStateUpdated",
                        state,
                        cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing colony simulation tick");
                }
            }
        }
    }
}
