using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace WeatherPlugin.WeatherProviders
{
    public class OpenWeatherMapMain
    {
        [JsonProperty( PropertyName = "temp" )]
        public double Temperature { get; set; }

        [JsonProperty( PropertyName = "humidity" )]
        public int Humidity { get; set; }
    }

    public class OpenWeatherMapWind
    {
        [JsonProperty( PropertyName = "speed" )]
        public double Speed { get; set; }
    }

    public class OpenWeatherMapResponse
    {
        [JsonProperty( PropertyName = "main" )]
        public OpenWeatherMapMain Main { get; set; }

        [JsonProperty( PropertyName = "wind" )]
        public OpenWeatherMapWind Wind { get; set; }
    }

    public class OpenWeatherMap : WeatherProvider
    {
        private static readonly HttpClient http = new HttpClient();
        private static readonly string _appId = "f46d03fa1a35bac20d7544198e219119";

        static OpenWeatherMap()
        {
            http.DefaultRequestHeaders.Add( "Accept-Encoding", "deflate, gzip" );
            http.Timeout = TimeSpan.FromSeconds( 10 );
        }

        public override WeatherResponse GetCurrentWeather()
        {
            GetCurrentGeoCoordinates( out var latitude, out var longitude );
            var request = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&APPID={_appId}&units=metric";
            var response = http.GetStringAsync( request ).Result;
            var gismeteoResponse = JsonConvert.DeserializeObject<OpenWeatherMapResponse>( response );
            return new WeatherResponse
            {
                Temperature = gismeteoResponse.Main.Temperature,
                Humidity = gismeteoResponse.Main.Humidity,
                WindSpeed = gismeteoResponse.Wind.Speed
            };
        }
    }
}
