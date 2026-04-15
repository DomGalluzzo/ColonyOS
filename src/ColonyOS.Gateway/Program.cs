using ColonyOS.Gateway.Configuration;
using ColonyOS.Gateway.Constants;
using ColonyOS.Gateway.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var colonyStateBaseUrl =
    builder.Configuration[$"Services:{MicroserviceConstants.Services.ColonyState}:BaseUrl"]
    ?? throw new InvalidOperationException(
        $"Configuration value Services:{MicroserviceConstants.Services.ColonyState}:BaseUrl is required.");

builder.Services.AddHttpClient<IColonyStateGatewayClient, ColonyStateGatewayClient>(client =>
{
    client.BaseAddress = new Uri(colonyStateBaseUrl);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;