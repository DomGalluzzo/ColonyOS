using System.Text.Json.Serialization;
using ColonyOS.ColonyStateService.Services;
using ColonyOS.ColonyStateService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAlertsService, AlertsService>();
builder.Services.AddSingleton<IColonyStateService, ColonyStateService>();
builder.Services.AddSingleton<ITaskService, TaskService>();
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

app.MapControllers();

app.Run();

public partial class Program { }