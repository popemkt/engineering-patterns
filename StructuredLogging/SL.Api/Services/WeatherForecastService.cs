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
        logger.LogInformation("Start getting forecast for {date}", startDate);
        
        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            rng.Next(-20, 55),
            _summaries[rng.Next(_summaries.Length)]
        ));
    }
}