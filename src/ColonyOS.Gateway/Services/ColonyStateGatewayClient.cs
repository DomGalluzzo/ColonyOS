using ColonyOS.Contracts.Models;
using ColonyOS.Gateway.Constants;

namespace ColonyOS.Gateway.Services
{
    public class ColonyStateGatewayClient : IColonyStateGatewayClient
    {
        private readonly HttpClient _httpClient;

        public ColonyStateGatewayClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(MicroserviceConstants.Routes.ColonyState, cancellationToken);
            response.EnsureSuccessStatusCode();

            var state = await response.Content.ReadFromJsonAsync<ColonyState>(cancellationToken);

            if (state is null)
                throw new InvalidOperationException("Colony state response was empty.");

            return state;
        }
    }
}
