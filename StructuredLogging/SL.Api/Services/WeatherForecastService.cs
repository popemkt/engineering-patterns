using Serilog.Context;

namespace SL.Api.Services;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> GetForecast(DateTime startDate);
}

public class WeatherForecastService(ILogger<WeatherForecastService> logger) : IWeatherForecastService
{
    private readonly string[] _summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public IEnumerable<WeatherForecast> GetForecast(DateTime startDate)
    {
        //DOC Using LogContext to add properties to the log context
        using var _ = LogContext.PushProperty("method-name", nameof(GetForecast));
        logger.LogInformation("Start getting forecast for {date}", startDate);

        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index =>
        {
            var forecast = new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                rng.Next(-20, 55),
                _summaries[rng.Next(_summaries.Length)]
            );
            logger.LogInformation("Generated forecast of {forecast}", forecast);
            return forecast;
        });
    }
}