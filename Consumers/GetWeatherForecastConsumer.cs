using MassTransit;

namespace DataService.Consumers;
public class GetWeatherForecastConsumer : IConsumer<GetWeatherForecasts>
{
    private static int deg = 0;
    private static readonly string[] Summaries = new[] 
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public async Task Consume(ConsumeContext<GetWeatherForecasts> context)
    {
        
        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC =  deg,// Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();
        deg++;
        await context.RespondAsync<WeatherForecasts>(new
        {
            Forecasts = forecasts
        });
    }
}

