using ColonyOS.Gateway.Hubs;
using ColonyOS.Gateway.Constants;
using ColonyOS.Gateway.Services;
using ColonyOS.Gateway.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:64030")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var colonyStateBaseUrl =
    builder.Configuration[$"Services:{MicroserviceConstants.Services.ColonyState}:BaseUrl"]
    ?? throw new InvalidOperationException(
        $"Configuration value Services:{MicroserviceConstants.Services.ColonyState}:BaseUrl is required.");

builder.Services.AddHttpClient<IColonyStateGatewayClient, ColonyStateGatewayClient>(client =>
{
    client.BaseAddress = new Uri(colonyStateBaseUrl);
});

builder.Services.AddHostedService<ColonyDashboardBroadcastWorker>();
builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthorization();


app.MapControllers();
app.MapHub<ColonyDashboardHub>("/hubs/colony-dashboard");

app.Run();

public partial class Program;