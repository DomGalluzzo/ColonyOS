using System.Text.Json;
using System.Text.Json.Serialization;
using ColonyOS.ColonyStateService.Models.ColonyState;
using ColonyOS.ColonyStateService.Models.Requests;
using ColonyOS.Contracts.Models.Alerts;
using ColonyOS.Contracts.Models.Tasks;
using ColonyOS.Gateway.Constants;

namespace ColonyOS.Gateway.Services
{
    public class ColonyStateGatewayClient : IColonyStateGatewayClient
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        public ColonyStateGatewayClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ColonyState> GetCurrentStateAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(MicroserviceConstants.Routes.ColonyState, cancellationToken);
            response.EnsureSuccessStatusCode();

            var colonyState = await response.Content.ReadFromJsonAsync<ColonyState>(_jsonSerializerOptions, cancellationToken);

            return colonyState ?? throw new InvalidOperationException("Colony state response was empty.");

        }

        public async Task<IReadOnlyCollection<Alert>> GetAlertsAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(MicroserviceConstants.Routes.Alerts, cancellationToken);
            response.EnsureSuccessStatusCode();

            var alerts = await response.Content.ReadFromJsonAsync<List<Alert>>(_jsonSerializerOptions, cancellationToken);

            return alerts ?? new List<Alert>();
        }

        public async Task<bool> AcknowledgeAlertAsync(Guid alertId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsync($"{MicroserviceConstants.Routes.Alerts}/{alertId}/acknowledge", null, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<List<TaskItem>> GetActiveTasksAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"{MicroserviceConstants.Routes.Tasks}", cancellationToken);
            response.EnsureSuccessStatusCode();

            var tasks = await response.Content.ReadFromJsonAsync<List<TaskItem>>(_jsonSerializerOptions, cancellationToken);

            return tasks ?? new List<TaskItem>();
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem, CancellationToken cancellationToken)
        {
            var taskRequest = new CreateTaskRequest()
            {
                Title = taskItem.Title,
                Description = taskItem.Description,
                TaskPriority = taskItem.TaskPriority,
                TargetSubsystem = taskItem.TargetSystem,
                TaskType = taskItem.TaskType,
                EstimatedDurationMinutes = taskItem.EstimatedDurationMinutes,
                SourceAlertId = taskItem.SourceAlertId
            };
            var response = await _httpClient.PostAsJsonAsync($"{MicroserviceConstants.Routes.Tasks}", taskRequest, cancellationToken);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<TaskItem>(cancellationToken);
            return content;
        }
    }
}
