using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace WeatherPlugin.WeatherProviders
{
    public class GismeteoTemperature
    {
        [JsonProperty( PropertyName = "C" )]
        public double Value { get; set; }
    }

    public class GismeteoHumidity
    {
        [JsonProperty( PropertyName = "percent" )]
        public int Percent;
    }

    public class GismeteoWind
    {
        [JsonProperty( PropertyName = "m_s" )]
        public double Speed { get; set; }
    }

    public class GismeteoResponse
    {
        [JsonProperty( PropertyName = "temperature" )]
        public GismeteoTemperature Temperature { get; set; }

        [JsonProperty( PropertyName = "humidity" )]
        public GismeteoHumidity Humidity { get; set; }

        [JsonProperty( PropertyName = "wind" )]
        public GismeteoWind Wind { get; set; }
    }

    public class GismeteoWeather : WeatherProvider
    {
        private static readonly HttpClient http = new HttpClient();

        static GismeteoWeather()
        {
            http.DefaultRequestHeaders.Add( "X-Gismeteo-Token", $"56b30cb255.3443075" );
            http.DefaultRequestHeaders.Add( "Accept-Encoding", "deflate, gzip" );
            http.Timeout = TimeSpan.FromSeconds( 10 );
        }

        public override WeatherResponse GetCurrentWeather()
        {
            GetCurrentGeoCoordinates( out var latitude, out var longitude );
            var response = http.GetStringAsync( $"https://api.gismeteo.net/v2/weather/current/?latitude={latitude}&longitude={longitude}" ).Result;
            var gismeteoResponse = JsonConvert.DeserializeObject<GismeteoResponse>( response );
            return new WeatherResponse
            {
                Temperature = gismeteoResponse.Temperature.Value,
                Humidity = gismeteoResponse.Humidity.Percent,
                WindSpeed = gismeteoResponse.Wind.Speed
            };
        }
    }
}
