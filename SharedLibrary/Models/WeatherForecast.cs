using ProtoBuf;

namespace SharedLibrary.Models
{
    [ProtoContract]
    public sealed class WeatherForecast
    {
        [ProtoMember(1)]
        public DateTime Date { get; set; }

        [ProtoMember(2)]
        public int TemperatureC { get; set; }

        [ProtoMember(3)]
        public string? Summary { get; set; }

        [ProtoIgnore]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
