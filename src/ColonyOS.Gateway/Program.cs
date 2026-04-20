using System.Text.Json.Serialization;
using ColonyOS.Gateway.Configuration;
using ColonyOS.Gateway.Constants;
using ColonyOS.Gateway.Services;

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
            .AllowAnyMethod();
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

builder.Services.AddControllers();
    //.AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    //});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAngular");
app.MapControllers();

app.Run();

public partial class Program;