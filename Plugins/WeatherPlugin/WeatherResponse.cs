
namespace WeatherPlugin
{
    public class WeatherResponse
    {
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }

        public string ToHumanString()
        {
            return $"Температура воздуха - {Temperature} градусов цельсия, Влажность - {Humidity} процентов, Скорость ветра {WindSpeed} метров в секунду";
        }
    }
}
