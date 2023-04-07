using ProtoBuf.Grpc.Configuration;
using SharedLibrary.Models;

namespace SharedLibrary.Services
{
    [Service]
    public interface IWeatherForecastService
    {
        public ValueTask<List<WeatherForecast>> GetWeatherForecastsAsync();
    }
}
