using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Exceptions;
using SL.Api.Services;

var builder = WebApplication.CreateBuilder(args);
//Serilog will replace default providers, ILogger will use Serilog's implementation
builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
var loggerSink = Environment.GetEnvironmentVariable("LOGGER_PROVIDER");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"{loggerSink}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .CreateLogger();
//There's an overload of CreateBootstrapLogger for bootstrap (aka not yet built app) logging only, then we can
//override with another if needed (one that can use DI etc), see  
try
{
    Log.Information("Start building host at {time}", DateTime.Now);
    var app = builder.Build();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapGet("/weatherforecast",
            ([FromServices] IWeatherForecastService service) => service.GetForecast(DateTime.Now))
        .WithOpenApi();
    Log.Information("Start running host at {time}", DateTime.Now);
    app.Run();
    Log.Information("Host terminated at {time}", DateTime.Now);
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}